using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using cursework.Models;
using cursework.ViewModels;

namespace cursework.Views;

public partial class LibraryView : UserControl
{
    public Library? CurrLib = null;
    private Dictionary<Key, bool> _keyStates = new();
    public Collection? SelectedCollection = null;
    
    public LibraryView()
    {
        InitializeComponent();
        
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
        this.CurrLib = new Library();
    }
    
    
    private void UpdateView()
    {
        Filters.Text = LibraryViewModel.FiltersString;
        ItemsGrid.ItemsSource = App.MainWindow.CurrLibView.CurrLib?.Filtered(LibraryViewModel.Filters);
    }
    private void ItemsGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        
        SelectedButtons.IsVisible = true;
        
        if (sender == null)
        {
            SelectedButtons.IsVisible = false;
            return;
        }
        
        var selected = ((Collection)((DataGrid)sender).SelectedItem);

        if (selected == null)
        {
            SelectedButtons.IsVisible = false;
            return;
        }

        this.SelectedCollection = selected;
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

        Library.Serialize(this.CurrLib, path[0]);
    }

    private void ApplyFilter()
    {
        List<string> filters = [];
        
        if(!string.IsNullOrEmpty(Universal.Text)) filters.Add(Universal.Text);
        
        LibraryViewModel.Filters.AddRange(filters);
        
        Universal.Text = null;
        UpdateView();
    }

    private void GoBack()
    {
        App.MainWindow.MainContent.Content = App.MainWindow.CurrHomeView;
    }
    
    private void ApplyFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        ApplyFilter();
    }
    private void ClearFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        LibraryViewModel.Filters.Clear();
        LibraryViewModel.Filters.Add("");
        UpdateView();

        Universal.Text = null;
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
            ApplyFilter();
        }

        if (Escape)
        {
            GoBack();
        }
    }

    private void Create_OnClick(object? sender, RoutedEventArgs e)
    {
        this.CurrLib?.Collections.Add(new Collection());
        UpdateView();
    }
    private void Open_OnClick(object? sender, RoutedEventArgs e)
    {
        if(this.SelectedCollection == null) return;
        
        App.MainWindow.CurrCollectionView = new CollectionView();
        App.MainWindow.CurrCollectionView.CurrCollection = this.SelectedCollection;
        App.MainWindow.MainContent.Content = App.MainWindow.CurrCollectionView;
    }
    private void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        if(this.SelectedCollection != null) this.CurrLib?.Collections.Remove(this.SelectedCollection);
        UpdateView();
    }
}