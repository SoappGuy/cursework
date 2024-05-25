using System;
using System.Collections.Generic;
using cursework.Models;

namespace cursework.ViewModels;

public class LibraryViewModel : ViewModelBase
{
    public static List<string> Filters = [""];
    public static List<Collection> CurrView
    {
        get
        {
            return App.MainWindow.CurrLibView.CurrLib?.Filtered(Filters);
        }
    }
}