using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the login page, handling user authentication with email and password credentials.
/// </summary>
public partial class LoginViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for user authentication.</param>
    public LoginViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Gets or sets the email address entered by the user.
    /// </summary>
    [ObservableProperty]
    private string email = string.Empty;

    /// <summary>
    /// Gets or sets the password entered by the user.
    /// </summary>
    [ObservableProperty]
    private string password = string.Empty;

    /// <summary>
    /// Gets or sets the validation error message displayed to the user.
    /// </summary>
    [ObservableProperty]
    private string errorMessage = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether a validation or authentication error is displayed.
    /// </summary>
    [ObservableProperty]
    private bool hasError;

    /// <summary>
    /// Validates the login credentials and authenticates the user. Navigates to the home page on success.
    /// </summary>
    [RelayCommand]
    private async Task LoginAsync()
    {
        HasError = false;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please fill in all fields.";
            HasError = true;
            return;
        }

        IsBusy = true;
        try
        {
            var (success, message, user) = await _dataService.LoginAsync(Email, Password);
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
    /// Navigates to the registration page for creating a new account.
    /// </summary>
    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await Shell.Current.GoToAsync("register");
    }
}
