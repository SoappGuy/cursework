using System;
using System.Collections.Generic;
using System.Linq;
using cursework.Models;

namespace cursework.ViewModels;

public class CollectionsViewModel
{
    public static Dictionary<string, object> Filters = new() {{"universal", ""}};
    public static Genre[] Genres { get; set; } = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToArray();

    public static List<Film> CurrView
    {
        get
        {
            return App.MainWindow.CurrCollectionView.CurrCollection?.Filtered(Filters);
        }
    }

    public static string CurrTitle => (App.MainWindow.CurrCollectionView.CurrCollection?.Title ?? "Films") + ":";
}