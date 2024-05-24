using Avalonia.Controls;
using cursework.Models;

namespace cursework.Views;

public partial class MainWindow : Window
{
    public LibraryView CurrLibView = null;
    public CollectionView CurrCollectionView = null;
    public FilmView CurrFilmView = null;
    
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new HomeView();
    }
}