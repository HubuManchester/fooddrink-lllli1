using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the meal planner page where users can organize and view their planned meals.
/// </summary>
public partial class MealPlanPage : ContentPage
{
    private readonly MealPlanViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MealPlanPage"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The meal plan view model that provides meal planning data.</param>
    public MealPlanPage(MealPlanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    /// <summary>
    /// Called when the page is navigated to. Loads meal plans from the view model.
    /// </summary>
    /// <param name="args">The navigation event arguments.</param>
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        try { await _viewModel.LoadMealPlansCommand.ExecuteAsync(null); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[FoodLens] MealPlanPage.OnNavigatedTo: {ex.Message}"); }
    }
}
