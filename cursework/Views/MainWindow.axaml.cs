using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using cursework.Models;

namespace cursework.Views;

public partial class MainWindow : Window
{
    public LibraryView CurrLibView = null;
    public CollectionView CurrCollectionView = null;
    public FilmView CurrFilmView = null;
    public HomeView CurrHomeView = null;
    public string Path = "";
    
    public MainWindow()
    {
        InitializeComponent();
        this.CurrHomeView = new HomeView();
        MainContent.Content = this.CurrHomeView;
    }

    public async Task<bool> GetPath()
    {
        var dialog = new SaveFileDialog()
        {
            Title = "Save File",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Library files", Extensions = new List<string> { "library" } },
            },
            DefaultExtension = "library",
            InitialFileName = "New Lib.library"
        };

        var path = await dialog.ShowAsync(App.MainWindow);

        if (path != null)
        {
            this.Path = path;
            return true;
        }

        return false;
    }
    
    public void Save()
    {
        Library.Serialize(this.CurrLibView.CurrLib, App.MainWindow.Path);
    }

    public async void SaveAs()
    {
        if (await this.GetPath())
        {
            Library.Serialize(this.CurrLibView.CurrLib, App.MainWindow.Path);
        }
    }
}