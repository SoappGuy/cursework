using System;
using Avalonia.Controls;
using cursework.Models;

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
        Console.WriteLine(selected.Title);
    }
}