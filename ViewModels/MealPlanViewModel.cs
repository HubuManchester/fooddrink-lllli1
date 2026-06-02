using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the meal planner page, allowing users to select dates, assign recipes
/// to specific meal types (Breakfast, Lunch, Dinner, Snack), and manage their meal plans.
/// </summary>
public partial class MealPlanViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Gets or sets the currently selected date for viewing and managing meal plans.
    /// </summary>
    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    /// <summary>
    /// Gets or sets the collection of meal plans for the selected date.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MealPlan> mealPlans = new();

    /// <summary>
    /// Gets or sets a value indicating whether there are no meal plans for the selected date.
    /// </summary>
    [ObservableProperty]
    private bool isEmpty;

    /// <summary>
    /// Gets or sets the collection of all available recipes for the recipe picker.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Recipe> availableRecipes = new();

    /// <summary>
    /// Gets or sets the recipe selected by the user in the recipe picker for adding to a meal plan.
    /// </summary>
    [ObservableProperty]
    private Recipe? selectedRecipe;

    /// <summary>
    /// Gets or sets the selected meal type for assigning a recipe (e.g., Breakfast, Lunch).
    /// </summary>
    [ObservableProperty]
    private string selectedMealType = "Lunch";

    /// <summary>
    /// Gets or sets a value indicating whether the recipe picker overlay is visible.
    /// </summary>
    [ObservableProperty]
    private bool isRecipePickerVisible;

    /// <summary>
    /// Gets the list of available meal type options.
    /// </summary>
    public List<string> MealTypes { get; } = new() { "Breakfast", "Lunch", "Dinner", "Snack" };

    /// <summary>
    /// Initializes a new instance of the <see cref="MealPlanViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for managing meal plans and recipes.</param>
    public MealPlanViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Meal Plan";
    }

    /// <summary>
    /// Called when <see cref="SelectedDate"/> changes; reloads meal plans for the new date.
    /// </summary>
    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadMealPlansAsync();
    }

    /// <summary>
    /// Loads all meal plans for the currently selected date.
    /// </summary>
    [RelayCommand]
    private async Task LoadMealPlansAsync()
    {
        IsBusy = true;
        try
        {
            var plans = await _dataService.GetMealPlansAsync(SelectedDate);
            MealPlans = new ObservableCollection<MealPlan>(plans);
            IsEmpty = MealPlans.Count == 0;
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Loads all available recipes and displays the recipe picker overlay.
    /// </summary>
    [RelayCommand]
    private async Task ShowAddRecipePickerAsync()
    {
        var recipes = await _dataService.GetRecipesAsync();
        AvailableRecipes = new ObservableCollection<Recipe>(recipes);
        IsRecipePickerVisible = true;
    }

    /// <summary>
    /// Adds the selected recipe to the meal plan with the chosen meal type and hides the picker.
    /// </summary>
    [RelayCommand]
    private async Task AddSelectedRecipeAsync()
    {
        if (SelectedRecipe is null) return;

        var plan = new MealPlan
        {
            Date = SelectedDate,
            MealType = SelectedMealType,
            RecipeId = SelectedRecipe.Id,
            Recipe = SelectedRecipe
        };

        await _dataService.AddMealPlanAsync(plan);
        IsRecipePickerVisible = false;
        SelectedRecipe = null;
        await LoadMealPlansAsync();
    }

    /// <summary>
    /// Dismisses the recipe picker without adding a recipe.
    /// </summary>
    [RelayCommand]
    private void CancelRecipePicker()
    {
        IsRecipePickerVisible = false;
        SelectedRecipe = null;
    }

    /// <summary>
    /// Deletes the specified meal plan entry and refreshes the meal plan list.
    /// </summary>
    /// <param name="plan">The meal plan entry to delete.</param>
    [RelayCommand]
    private async Task DeleteMealPlanAsync(MealPlan plan)
    {
        await _dataService.RemoveMealPlanAsync(plan.Id);
        await LoadMealPlansAsync();
    }

    /// <summary>
    /// Navigates to the recipe detail page for the specified recipe.
    /// </summary>
    /// <param name="recipeId">The unique identifier of the recipe to view.</param>
    [RelayCommand]
    private static async Task GoToRecipeDetailAsync(int recipeId)
    {
        await Shell.Current.GoToAsync($"recipedetail?id={recipeId}");
    }
}
