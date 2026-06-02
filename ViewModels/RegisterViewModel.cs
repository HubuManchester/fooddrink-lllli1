using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the registration page, handling new user account creation with validation
/// for display name, email, password, and password confirmation.
/// </summary>
public partial class RegisterViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for user registration.</param>
    public RegisterViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Gets or sets the display name entered by the user for the new account.
    /// </summary>
    [ObservableProperty]
    private string displayName = string.Empty;

    /// <summary>
    /// Gets or sets the email address entered by the user for the new account.
    /// </summary>
    [ObservableProperty]
    private string email = string.Empty;

    /// <summary>
    /// Gets or sets the password entered by the user for the new account.
    /// </summary>
    [ObservableProperty]
    private string password = string.Empty;

    /// <summary>
    /// Gets or sets the password confirmation entered by the user.
    /// </summary>
    [ObservableProperty]
    private string confirmPassword = string.Empty;

    /// <summary>
    /// Gets or sets the validation error message displayed to the user.
    /// </summary>
    [ObservableProperty]
    private string errorMessage = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether a validation or registration error is displayed.
    /// </summary>
    [ObservableProperty]
    private bool hasError;

    /// <summary>
    /// Validates all registration fields and creates a new user account. Navigates to the home page on success.
    /// </summary>
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

        if (!Email.Contains("@") || !Email.Contains("."))
        {
            ErrorMessage = "Please enter a valid email address.";
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
            var (success, message, user) = await _dataService.RegisterAsync(DisplayName, Email, Password);
            if (success)
            {
                await Shell.Current.GoToAsync("//home");
            }
            else
            {
                ErrorMessage = message;
                HasError = true;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Navigates back to the login page.
    /// </summary>
    [RelayCommand]
    private async Task GoToLoginAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
