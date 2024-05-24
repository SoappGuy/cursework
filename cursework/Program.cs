using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using cursework.Models;

namespace cursework;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        // var collection = Models.Collection.Deserialize("/home/dumbnerd/Downloads/films.collection").Unwrap();
        // var lib = new Library();
        // lib.Add(collection);
        //
        // Library.Serialize(lib, "/home/dumbnerd/Downloads/", "films.library");

        // var set = collection.Films.ToList();
        // collection.Search("sciencefiction");
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}