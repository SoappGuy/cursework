using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using cursework.Models;
using cursework.ViewModels;

namespace cursework.Views;

public partial class LibraryView : UserControl
{
    public Library? CurrLib = null;
    
    public LibraryView()
    {
        InitializeComponent();
        this.CurrLib = new Library();
    }
    
    private void ItemsGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selected = ((Collection)((DataGrid)sender).SelectedItem);
        // Console.WriteLine(selected.Title);
    }
    
    private async void OpenButton_OnClick(object? sender, RoutedEventArgs e)
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
        
        var result = await dialog.ShowAsync(App.MainWindow);
        
        if (result != null && result.Length > 0)
        {
            App.MainWindow.CurrLibView = new LibraryView();
            App.MainWindow.CurrLibView.CurrLib = Library.Deserialize(result[0]).Unwrap();
            
            App.MainWindow.MainContent.Content = App.MainWindow.CurrLibView;
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

        Library.Serialize(this.CurrLib, path[0]);
    }

    private void ApplyFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        List<string> filters = [];
        
        if(!string.IsNullOrEmpty(Description.Text)) filters.Add(Description.Text);
        if(!string.IsNullOrEmpty(Universal.Text))   filters.Add(Universal.Text);
        if(!string.IsNullOrEmpty(Title.Text))       filters.Add(Title.Text);
        
        LibraryViewModel.Filters.AddRange(filters);
        ItemsGrid.ItemsSource = App.MainWindow.CurrLibView.CurrLib?.Filtered(LibraryViewModel.Filters);
    }

    private void ClearFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        LibraryViewModel.Filters.Clear();
        LibraryViewModel.Filters.Add("");
        ItemsGrid.ItemsSource = App.MainWindow.CurrLibView.CurrLib?.Filtered(LibraryViewModel.Filters);
    }
}