using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the settings page with options for account management, appearance, and location preferences.
/// </summary>
public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class with the specified view model.
    /// Configures the dark mode toggle to reflect the current application theme.
    /// </summary>
    /// <param name="viewModel">The settings view model that provides user settings data.</param>
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        DarkModeSwitch.IsToggled = Application.Current!.UserAppTheme == AppTheme.Dark;
        DarkModeSwitch.Toggled += OnDarkModeToggled;
    }

    /// <summary>
    /// Called when the page is navigated to. Loads the current user's settings data.
    /// </summary>
    /// <param name="args">The navigation event arguments.</param>
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        try { await _viewModel.LoadUserAsync(); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[FoodLens] SettingsPage.OnNavigatedTo: {ex.Message}"); }
    }

    /// <summary>
    /// Handles the dark mode toggle switch event, applying the selected theme to the application.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The toggled event data containing the new switch value.</param>
    private void OnDarkModeToggled(object? sender, ToggledEventArgs e)
    {
        Application.Current!.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }
}
