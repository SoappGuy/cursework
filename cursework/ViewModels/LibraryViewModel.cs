using System;
using System.Collections.Generic;
using System.Linq;
using cursework.Models;

namespace cursework.ViewModels;

public class LibraryViewModel : ViewModelBase
{
    public static List<string> Filters = [""];

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
    public static List<Collection> CurrView
    {
        get
        {
            return App.MainWindow.CurrLibView.CurrLib?.Filtered(Filters);
        }
    }
}