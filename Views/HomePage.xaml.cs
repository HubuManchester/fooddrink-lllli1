using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
