using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        DarkModeSwitch.IsToggled = Application.Current!.UserAppTheme == AppTheme.Dark;
        DarkModeSwitch.Toggled += OnDarkModeToggled;
    }

    private void OnDarkModeToggled(object? sender, ToggledEventArgs e)
    {
        Application.Current!.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }
}
