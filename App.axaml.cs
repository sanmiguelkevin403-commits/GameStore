using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GameStore.Data;
using GameStore.Services;

namespace GameStore;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var database = new GameDatabase();
            database.Initialize();
            var service = new GameService(new GameRepository(database));
            desktop.MainWindow = new Views.MainWindow(service);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
