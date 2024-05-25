using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using cursework.Models;
using cursework.ViewModels;

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
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        this._keyStates[e.Key] = true;

        this._keyStates.TryGetValue(Key.Enter, out bool Enter);
        this._keyStates.TryGetValue(Key.Escape, out bool Escape);
        if (Enter)
        {
            // ApplyFilter();
        }

        if (Escape)
        {
            GoBack();
        }
    }
    
    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        
        var dialog = new OpenFileDialog
        {
            Title = "Open File",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Library files", Extensions = new List<string> { "library" } },
            },
            AllowMultiple = false,
        };

        var path = await dialog.ShowAsync(App.MainWindow);
        
        Library.Serialize(App.MainWindow.CurrLibView.CurrLib, path[0]);
    }
    
    private void UpdateView()
    {
        Filters.Text = CollectionsViewModel.FiltersString;
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

    private void ApplyFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        ApplyFilter();
    }
    
    private void ApplyFilter()
    {
        List<string> filters = [];
        
        if(!string.IsNullOrEmpty(Universal.Text)) filters.Add(Universal.Text);
        
        CollectionsViewModel.Filters.AddRange(filters);
        
        Universal.Text = null;
        UpdateView();
    }

    private void ClearFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        CollectionsViewModel.Filters.Clear();
        CollectionsViewModel.Filters.Add("");
        UpdateView();

        Universal.Text = null;
    }
}