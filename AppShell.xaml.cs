using FoodLens.Services;
using FoodLens.ViewModels;

namespace FoodLens;

/// <summary>
/// Main application shell providing tab-based and flyout navigation with dynamic login/logout menu.
/// </summary>
public partial class AppShell : Shell
{
    private bool _isLoggedIn;

    public AppShell()
    {
        InitializeComponent();
        Navigated += OnNavigated;
    }

    /// <summary>
    /// Updates the login/logout flyout menu item based on current user state.
    /// </summary>
    public async Task UpdateLoginMenuAsync()
    {
        var dataService = BaseViewModel.GetServiceProvider()?.GetService<IDataService>();
        if (dataService is null) return;

        var user = await dataService.GetCurrentUserAsync();
        _isLoggedIn = user is not null;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoginFlyoutItem.Title = _isLoggedIn ? "Logout" : "Login";
            LoginFlyoutItem.Icon = _isLoggedIn ? "logout.svg" : "login.svg";
        });
    }

    /// <summary>
    /// Intercepts navigation to the login page when already logged in, performing logout instead.
    /// </summary>
    /// <param name="args">Navigation event arguments.</param>
    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        try
        {
            if (_isLoggedIn && args.Target.Location.OriginalString.Contains("login"))
            {
                args.Cancel();

                var dataService = BaseViewModel.GetServiceProvider()?.GetService<IDataService>();
                if (dataService is not null)
                {
                    await dataService.LogoutAsync();
                    await UpdateLoginMenuAsync();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FoodLens] OnNavigating error: {ex.Message}");
        }

        base.OnNavigating(args);
    }

    /// <summary>
    /// Updates the login menu state after each navigation.
    /// </summary>
    private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        try
        {
            await UpdateLoginMenuAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FoodLens] OnNavigated error: {ex.Message}");
        }
    }
}
