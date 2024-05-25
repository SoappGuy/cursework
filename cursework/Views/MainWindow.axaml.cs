using Avalonia.Controls;
using cursework.Models;

namespace cursework.Views;

public partial class MainWindow : Window
{
    public LibraryView CurrLibView = null;
    public CollectionView CurrCollectionView = null;
    public FilmView CurrFilmView = null;
    public HomeView CurrHomeView = null;
    
    public MainWindow()
    {
        InitializeComponent();
        this.CurrHomeView = new HomeView();
        MainContent.Content = this.CurrHomeView;
    }
}