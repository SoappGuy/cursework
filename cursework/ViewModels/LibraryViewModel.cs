using System;
using System.Collections.Generic;
using System.Linq;
using cursework.Models;

namespace cursework.ViewModels;

public class LibraryViewModel : ViewModelBase
{
    public static Dictionary<string, object> Filters = new() {{"universal", ""}};
    
    public static List<Collection> CurrView
    {
        get
        {
            return App.MainWindow.CurrLibView.CurrLib?.Filtered(Filters);
        }
    }
}