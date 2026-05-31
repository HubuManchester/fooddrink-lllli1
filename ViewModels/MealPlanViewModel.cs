using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class MealPlanViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private ObservableCollection<MealPlan> mealPlans = new();

    [ObservableProperty]
    private bool isEmpty;

    [ObservableProperty]
    private ObservableCollection<Recipe> availableRecipes = new();

    [ObservableProperty]
    private Recipe? selectedRecipe;

    [ObservableProperty]
    private string selectedMealType = "Lunch";

    [ObservableProperty]
    private bool isRecipePickerVisible;

    public List<string> MealTypes { get; } = new() { "Breakfast", "Lunch", "Dinner", "Snack" };

    public MealPlanViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Meal Plan";
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadMealPlansAsync();
    }

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

    [RelayCommand]
    private async Task ShowAddRecipePickerAsync()
    {
        var recipes = await _dataService.GetRecipesAsync();
        AvailableRecipes = new ObservableCollection<Recipe>(recipes);
        IsRecipePickerVisible = true;
    }

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

    [RelayCommand]
    private void CancelRecipePicker()
    {
        IsRecipePickerVisible = false;
        SelectedRecipe = null;
    }

    [RelayCommand]
    private async Task DeleteMealPlanAsync(MealPlan plan)
    {
        await _dataService.RemoveMealPlanAsync(plan.Id);
        await LoadMealPlansAsync();
    }

    [RelayCommand]
    private static async Task GoToRecipeDetailAsync(int recipeId)
    {
        await Shell.Current.GoToAsync($"///recipedetail?id={recipeId}");
    }
}
