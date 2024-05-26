using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using cursework.Models;
using cursework.ViewModels;
using DynamicData;

namespace cursework.Views;

public partial class CollectionView : UserControl
{
    public Collection? CurrCollection = null;
    private Dictionary<Key, bool> _keyStates = new();
    public Film? SelectedFilm = null;
    
    public CollectionView()
    {
        InitializeComponent();   
        
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
        
        this.CurrCollection = new Collection();
        
        DateSearchStart.SelectedDate = new DateTimeOffset(new DateTime(1900, 1, 1));
        DateSearchEnd  .SelectedDate = new DateTimeOffset(DateTime.Today);
        // this.Classes.Add("Hidden");
    }
    
    private void ItemsGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        
        SelectedButtons.IsVisible = true;
        
        if (sender == null)
        {
            SelectedButtons.IsVisible = false;
            return;
        }
        
        var selected = ((Film)((DataGrid)sender).SelectedItem);

        if (selected == null)
        {
            SelectedButtons.IsVisible = false;
            return;
        }

        this.SelectedFilm = selected;
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
        // Filters.Text = CollectionsViewModel.FiltersString;
        ItemsGrid.ItemsSource = App.MainWindow.CurrCollectionView.CurrCollection?.Filtered(CollectionsViewModel.Filters);
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
        throw new NotImplementedException();
        // if(this.SelectedCollection == null) return;
        //
        // App.MainWindow.CurrCollectionView = new CollectionView();
        // App.MainWindow.CurrCollectionView.CurrCollection = this.SelectedCollection;
        // App.MainWindow.MainContent.Content = App.MainWindow.CurrCollectionView;
    }
    private void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        if(this.SelectedFilm != null) this.CurrCollection?.Films.Remove(this.SelectedFilm);
        UpdateView();
    }

    private void Apply_OnClick(object? sender, RoutedEventArgs e)
    {
        Apply();
    }
    
    private void Apply()
    {
        Dictionary<string, object> filters = [];
        
        if(!string.IsNullOrEmpty(UniversalSearch.Text))   filters.Add("universal",   UniversalSearch.Text);
        if(!string.IsNullOrEmpty(TitleSearch.Text))       filters.Add("Title",       TitleSearch.Text);
        if(!string.IsNullOrEmpty(DescriptionSearch.Text)) filters.Add("Description", DescriptionSearch.Text);
        if(!string.IsNullOrEmpty(StudioSearch.Text))      filters.Add("Studio",      StudioSearch.Text);
        if(!string.IsNullOrEmpty(DirectorSearch.Text))    filters.Add("Director",    DirectorSearch.Text);
        if(GenreSearch.SelectedItem != null)              filters.Add("Genre",       GenreSearch.SelectedItem);
        if(RateSearch.Value != null)                      filters.Add("Rate",        RateSearch.Value);
        if (DateSearchStart.SelectedDate != null &&
            DateSearchEnd.SelectedDate != null)
        {
            DateTimeOffset?[] dates = {DateSearchStart.SelectedDate, DateSearchEnd.SelectedDate};
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
        CollectionsViewModel.Filters = new Dictionary<string, object>() {{"universal", ""}};
        UpdateView();
    
        UniversalSearch  .Text         = null;
        TitleSearch      .Text         = null;
        DescriptionSearch.Text         = null;
        GenreSearch      .SelectedItem = null;
        StudioSearch     .Text         = null;
        DirectorSearch   .Text         = null;
        RateSearch       .Value        = null;
        DateSearchStart  .SelectedDate = new DateTimeOffset(new DateTime(1900, 1, 1));
        DateSearchEnd    .SelectedDate = new DateTimeOffset(DateTime.Today);
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

}