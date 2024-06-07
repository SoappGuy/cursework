using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using cursework.Models;
using cursework.ViewModels;
using LibVLCSharp.Shared;

namespace cursework.Views;

public partial class CollectionView : UserControl
{
    public Collection CurrCollection;
    private Dictionary<Key, bool> _keyStates = new();
    public Film? SelectedFilm = null;

    public CollectionView()
    {
        InitializeComponent();

        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;

        this.CurrCollection = new Collection();

        // DateSearchStart.SelectedDate = new DateTimeOffset(new DateTime(1900, 1, 1));
        // DateSearchEnd.SelectedDate = new DateTimeOffset(DateTime.Today);
    }

    private void FilmsList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        SelectedButtons.IsVisible = true;

        if (sender == null)
        {
            SelectedButtons.IsVisible = false;
            return;
        }

        var selected = ((Film)((ListBox)sender).SelectedItem);

        if (selected == null)
        {
            SelectedButtons.IsVisible = false;
            return;
        }

        this.SelectedFilm = selected;
        CollectionsViewModel.SelectedFilm = selected;
        ItemsGrid.DataContext = CollectionsViewModel.SelectedFilm;
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        this._keyStates[e.Key] = false;
    }

    private async void OnKeyDown(object? sender, KeyEventArgs e)
    {
        this._keyStates[e.Key] = true;

        this._keyStates.TryGetValue(Key.Enter, out bool Enter);
        this._keyStates.TryGetValue(Key.Escape, out bool Escape);
        this._keyStates.TryGetValue(Key.S, out bool S);
        this._keyStates.TryGetValue(Key.LeftCtrl, out bool LeftCtrl);

        if (Enter)
        {
            Apply();
        }

        if (Escape)
        {
            GoBack();
        }

        if (LeftCtrl && S)
        {
            if (App.MainWindow.Path == "")
            {
                await App.MainWindow.GetPath();
            }

            App.MainWindow.Save();
        }
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        App.MainWindow.SaveAs();
    }

    private void UpdateView()
    {
        FilmSelector.ItemsSource =
            App.MainWindow.CurrCollectionView.CurrCollection?.Filtered(CollectionsViewModel.Filters);

        if (SelectedFilm == null) return;
        ItemsGrid.DataContext = SelectedFilm;
    }

    private void GoBack()
    {
        App.MainWindow.MainContent.Content = App.MainWindow.CurrLibView;
    }

    private void Create_OnClick(object? sender, RoutedEventArgs e)
    {
        this.CurrCollection?.Films.Add(new Film());
        UpdateView();
    }

    private void Open_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(SelectedFilm?.FilePath)) return;

        using var media = new Media(CollectionsViewModel.libvlc, SelectedFilm.FilePath);
        CollectionsViewModel.player.Play(media);
    }

    private void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.SelectedFilm != null) this.CurrCollection?.Films.Remove(this.SelectedFilm);
        UpdateView();
    }

    private void Apply_OnClick(object? sender, RoutedEventArgs e)
    {
        Apply();
    }

    private void Apply()
    {
        Dictionary<string, object> filters = [];

        if (!string.IsNullOrEmpty(UniversalSearch.Text)) filters.Add("universal", UniversalSearch.Text);
        if (!string.IsNullOrEmpty(TitleSearch.Text)) filters.Add("Title", TitleSearch.Text);
        if (!string.IsNullOrEmpty(DescriptionSearch.Text)) filters.Add("Description", DescriptionSearch.Text);
        if (!string.IsNullOrEmpty(StudioSearch.Text)) filters.Add("Studio", StudioSearch.Text);
        if (!string.IsNullOrEmpty(DirectorSearch.Text)) filters.Add("Director", DirectorSearch.Text);
        if (GenreSearch.SelectedItems != null) filters.Add("Genres", ((AvaloniaList<object>)GenreSearch.SelectedItems).ToList());
        if (RateSearchStart.Value != null ||
            RateSearchEnd.Value != null)
        {
            double?[] rates =
                { decimal.ToDouble(RateSearchStart?.Value ?? 0), decimal.ToDouble(RateSearchEnd?.Value ?? 0) };
            filters.Add("Rate", rates);
        }

        if (DateSearchStart.SelectedDate != null ||
            DateSearchEnd.SelectedDate != null)
        {
            DateTimeOffset?[] dates = { DateSearchStart.SelectedDate, DateSearchEnd.SelectedDate };
            filters.Add("ReleaseDate", dates);
        }

        CollectionsViewModel.Filters = filters;

        UpdateView();
    }

    private void Clear_OnClick(object? sender, RoutedEventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        CollectionsViewModel.Filters = new Dictionary<string, object>() { { "universal", "" } };
        UpdateView();

        UniversalSearch.Text = null;
        TitleSearch.Text = null;
        DescriptionSearch.Text = null;
        GenreSearch.SelectedItem = null;
        StudioSearch.Text = null;
        DirectorSearch.Text = null;
        RateSearchStart.Value = null;
        RateSearchEnd.Value = null;
        DateSearchStart.SelectedDate = null;
        DateSearchEnd.SelectedDate = null;
        // DateSearchStart.SelectedDate = new DateTimeOffset(new DateTime(1900, 1, 1));
        // DateSearchEnd.SelectedDate = new DateTimeOffset(DateTime.Today);
    }

    private void AccurateSearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender == null) return;

        var button = (ToggleButton)sender;
        if (button.IsChecked ?? false)
        {
            AccurateGrid.IsVisible = true;
            Search.Height = 200;
        }
        else
        {
            AccurateGrid.IsVisible = false;
            Search.Height = 20;
            Clear();
        }
    }

    private void PlayToggle_OnClick(object? sender, RoutedEventArgs e)
    {
        CollectionsViewModel.player.Pause();
    }

    private void MuteToggle_OnClick(object? sender, RoutedEventArgs e)
    {
        CollectionsViewModel.player.Mute = !CollectionsViewModel.player.Mute;
    }

    private async void Print_OnClick(object? sender, RoutedEventArgs e)
    {
        var textToPrint = "";

        if (CollectionsViewModel.Filters.Count == 1 && CollectionsViewModel.Filters.TryGetValue("universal", out var _))
        {
            textToPrint += "No selected filters\n";
        }
        else
        {
            textToPrint += "Filters:\n";
            foreach (var (prop, value) in CollectionsViewModel.Filters)
            {
                
                switch (prop)
                {
                    case "Genres":
                        var genres = (List<object>)value;
                        if(genres.Count != 0) textToPrint += $"Genres: {string.Join(", ", genres)}\n";
                        break;
                    case "ReleaseDate":

                        DateTimeOffset? startDate = ((DateTimeOffset?[])value)[0];
                        DateTimeOffset? endDate = ((DateTimeOffset?[])value)[1];

                        if (startDate != null)
                        {
                            textToPrint += $"Release Date: {startDate?.UtcDateTime.ToString("dd MMM yyyy")}";
                        }
                        
                        if (startDate != null && endDate != null)
                        {
                            textToPrint += " - ";
                        }
                        
                        if (endDate != null)
                        {
                            textToPrint += $"{endDate?.UtcDateTime.ToString("dd MMM yyyy")}";
                        }

                        textToPrint += "\n";
                        break;

                    case "Rate":
                        var startRate = ((double?[])value)[0] ?? 0;
                        var endRate = ((double?[])value)[1] ?? 10;

                        if ((startRate == 0 && endRate == 0) || (endRate < startRate))
                        {
                            endRate = 10;
                        }

                        if (!(startRate == 0 && endRate == 10))
                        {
                            textToPrint += $"Rate: {startRate} - {endRate}\n";
                        }

                        break;
                    default:
                        textToPrint += $"{prop}: {(string)value}\n";
                        break;
                }
            }
        }

        textToPrint += "\nFilms:\n";

        var films = App.MainWindow.CurrCollectionView.CurrCollection?.Filtered(CollectionsViewModel.Filters);
        if (films.Count == 0)
        {
            textToPrint += "No films found";
        }
        else
        {
            foreach (var film in films)
            {
                textToPrint += $"{film}\n";
            }
        }
        
        var dialog = new SaveFileDialog()
        {
            Title = "Save found films",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Text files", Extensions = new List<string> { "txt" } },
            },
            DefaultExtension = "txt",
            InitialFileName = "found_films.txt"
        };

        var path = await dialog.ShowAsync(App.MainWindow);
        
        if (path != null)
        {
            File.WriteAllTextAsync(path, textToPrint);
        }
    }
}