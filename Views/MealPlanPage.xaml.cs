using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class MealPlanPage : ContentPage
{
    private readonly MealPlanViewModel _viewModel;

    public MealPlanPage(MealPlanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _viewModel.LoadMealPlansCommand.ExecuteAsync(null);
    }
}
