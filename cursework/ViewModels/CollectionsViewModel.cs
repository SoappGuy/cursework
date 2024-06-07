using System;
using System.Collections.Generic;
using System.Linq;
using cursework.Models;
using LibVLCSharp.Shared;

namespace cursework.ViewModels;

public class CollectionsViewModel
{
    public static Dictionary<string, object> Filters = new() {{"universal", ""}};
    public static Genre[] Genres { get; set; } = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToArray();
    
    public static List<Film> CurrView
    {
        get
        {
            return App.MainWindow.CurrCollectionView.CurrCollection.Filtered(Filters);
        }
    }

    public static Film? SelectedFilm { get; set; } = CurrView.Count > 0 ? CurrView[0] : null;
    public static string CurrTitle => (App.MainWindow.CurrCollectionView.CurrCollection?.Title ?? "Films") + ":";
    
    public static LibVLC libvlc = new LibVLC();
    public static MediaPlayer player { get; set; } = new MediaPlayer(libvlc);
}
