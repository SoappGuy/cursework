using System.Collections.Generic;
using cursework.Models;

namespace cursework.ViewModels;

public class CollectionsViewModel
{
    public static List<string> Filters = [""];
    public static List<Film> CurrView
    {
        get
        {
            return App.MainWindow.CurrCollectionView.CurrCollection?.Filtered(Filters);
        }
    }
    public static string FiltersString
    {
        get
        {
            List<string> result = [];
            foreach (var filter in Filters)
            {
                if (filter == "") continue;
                result.Add($"[{filter}]");
            }
            return string.Join(", ", result);
        }
    }

    public static string CurrTitle => (App.MainWindow.CurrCollectionView.CurrCollection?.Title ?? "Films") + ":";
}