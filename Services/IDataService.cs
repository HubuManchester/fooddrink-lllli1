using FoodLens.Models;

namespace FoodLens.Services;

public interface IDataService
{
    Task InitializeAsync();
    Task<List<Recipe>> GetRecipesAsync();
    Task<Recipe?> GetRecipeByIdAsync(int id);
    Task<List<Recipe>> SearchRecipesAsync(string keyword, string? category = null);
    Task<List<Recipe>> GetFavoriteRecipesAsync();
    Task ToggleFavoriteAsync(int recipeId);
    Task<List<Category>> GetCategoriesAsync();
    Task AddMealPlanAsync(MealPlan mealPlan);
    Task<List<MealPlan>> GetMealPlansAsync(DateTime date);
    Task RemoveMealPlanAsync(int id);
}
