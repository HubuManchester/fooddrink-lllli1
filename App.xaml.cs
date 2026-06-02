using FoodLens.Helpers;

namespace FoodLens;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        PlatformHelper.StartMonitoring();
        var window = new Window(new AppShell());
        window.Destroying += OnWindowDestroying;
        return window;
    }

    private void OnWindowDestroying(object? sender, EventArgs e)
    {
        PlatformHelper.StopMonitoring();
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"[FoodLens] UnhandledException: {e.ExceptionObject}");
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"[FoodLens] UnobservedTaskException: {e.Exception}");
        e.SetObserved();
    }
}
