using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using cursework.Models;
using cursework.ViewModels;

namespace cursework.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();

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

            App.MainWindow.Path = result[0];
            App.MainWindow.MainContent.Content = App.MainWindow.CurrLibView;
        }
    }

    private void CreateButton_OnClick(object? sender, RoutedEventArgs e)
    {
        App.MainWindow.CurrLibView = new LibraryView();
        App.MainWindow.MainContent.Content = App.MainWindow.CurrLibView;
    }
}