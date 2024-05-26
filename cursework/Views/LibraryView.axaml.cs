using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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
    
    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        App.MainWindow.SaveAs();
    }

    private void GoBack()
    {
        App.MainWindow.MainContent.Content = App.MainWindow.CurrHomeView;
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
    }
    
    private void AccurateSearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender == null) return;
        
        var button = (ToggleButton)sender;
        if (button.IsChecked ?? false)
        {
            AccurateGrid.IsVisible = true;
            Search.Height = 100;
        }
        else
        {
            AccurateGrid.IsVisible = false;
            Search.Height = 20;
            Clear();
        }
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
        LibraryViewModel.Filters = filters;
        
        UpdateView();
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
                if (await App.MainWindow.GetPath())
                {
                    App.MainWindow.Save();
                }
            }
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