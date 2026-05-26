using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class MealPlanPage : ContentPage
{
    public MealPlanPage(MealPlanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
