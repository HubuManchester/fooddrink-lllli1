namespace FoodLens;

/// <summary>
/// Main application entry point. Configures global exception handling and creates the main window.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the application, registers global exception handlers.
    /// </summary>
    public App()
    {
        InitializeComponent();
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    /// <summary>
    /// Creates the main application window with the AppShell as the root page.
    /// </summary>
    /// <param name="activationState">The activation state from the platform.</param>
    /// <returns>A new Window instance.</returns>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    /// <summary>
    /// Handles unhandled exceptions from the AppDomain.
    /// </summary>
    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"[FoodLens] UnhandledException: {e.ExceptionObject}");
    }

    /// <summary>
    /// Handles unobserved task exceptions to prevent application crashes.
    /// </summary>
    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"[FoodLens] UnobservedTaskException: {e.Exception}");
        e.SetObserved();
    }
}
