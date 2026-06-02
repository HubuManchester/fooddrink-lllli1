using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the login page where users can authenticate with their credentials.
/// </summary>
public partial class LoginPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginPage"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The login view model that handles authentication logic.</param>
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
