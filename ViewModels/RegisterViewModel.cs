using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FoodLens.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    [ObservableProperty]
    private string displayName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool hasError;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        HasError = false;

        if (string.IsNullOrWhiteSpace(DisplayName) || string.IsNullOrWhiteSpace(Email)
            || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Please fill in all fields.";
            HasError = true;
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            HasError = true;
            return;
        }

        if (Password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters.";
            HasError = true;
            return;
        }

        IsBusy = true;
        try
        {
            await Task.Delay(500);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToLoginAsync()
    {
        await Shell.Current.GoToAsync("//login");
    }
}
