using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using cursework.ViewModels;
using cursework.Views;

namespace cursework;

public partial class App : Application
{
    public static MainWindow MainWindow;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var win = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            MainWindow = win;
            desktop.MainWindow = win;
        }

        base.OnFrameworkInitializationCompleted();
    }
}