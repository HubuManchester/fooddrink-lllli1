using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the registration page where new users can create an account.
/// </summary>
public partial class RegisterPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterPage"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The registration view model that handles user sign-up logic.</param>
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
