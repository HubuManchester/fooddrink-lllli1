namespace FoodLens.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        DarkModeSwitch.IsToggled = Application.Current!.UserAppTheme == AppTheme.Dark;
        DarkModeSwitch.Toggled += OnDarkModeToggled;
    }

    private void OnDarkModeToggled(object? sender, ToggledEventArgs e)
    {
        Application.Current!.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }
}
