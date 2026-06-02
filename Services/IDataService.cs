using FoodLens.Models;

namespace FoodLens.Services;

/// <summary>
/// Provides data access operations for recipes, meal plans, categories, and user authentication.
/// </summary>
public interface IDataService
{
    /// <summary>
    /// Initializes the database connection and creates tables if needed.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Retrieves all recipes from the database.
    /// </summary>
    Task<List<Recipe>> GetRecipesAsync();

    /// <summary>
    /// Retrieves a single recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The recipe ID.</param>
    /// <returns>The recipe if found; otherwise, null.</returns>
    Task<Recipe?> GetRecipeByIdAsync(int id);

    /// <summary>
    /// Searches recipes by keyword and optional category filter.
    /// </summary>
    /// <param name="keyword">Search text to match against title and description.</param>
    /// <param name="category">Optional category filter.</param>
    Task<List<Recipe>> SearchRecipesAsync(string keyword, string? category = null);

    /// <summary>
    /// Retrieves all recipes marked as favorites.
    /// </summary>
    Task<List<Recipe>> GetFavoriteRecipesAsync();

    /// <summary>
    /// Toggles the favorite status of a recipe.
    /// </summary>
    /// <param name="recipeId">The recipe ID to toggle.</param>
    Task ToggleFavoriteAsync(int recipeId);

    /// <summary>
    /// Adds a new recipe to the database.
    /// </summary>
    /// <param name="recipe">The recipe to add.</param>
    Task AddRecipeAsync(Recipe recipe);

    /// <summary>
    /// Updates an existing recipe in the database.
    /// </summary>
    /// <param name="recipe">The recipe with updated values.</param>
    Task UpdateRecipeAsync(Recipe recipe);

    /// <summary>
    /// Deletes a recipe and its associated meal plans from the database.
    /// </summary>
    /// <param name="recipeId">The recipe ID to delete.</param>
    Task DeleteRecipeAsync(int recipeId);

    /// <summary>
    /// Retrieves all available recipe categories.
    /// </summary>
    Task<List<Category>> GetCategoriesAsync();

    /// <summary>
    /// Adds a meal plan entry to the database.
    /// </summary>
    /// <param name="mealPlan">The meal plan to add.</param>
    Task AddMealPlanAsync(MealPlan mealPlan);

    /// <summary>
    /// Retrieves all meal plans for a specific date.
    /// </summary>
    /// <param name="date">The date to filter by.</param>
    Task<List<MealPlan>> GetMealPlansAsync(DateTime date);

    /// <summary>
    /// Removes a meal plan entry by its ID.
    /// </summary>
    /// <param name="id">The meal plan ID to remove.</param>
    Task RemoveMealPlanAsync(int id);

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="displayName">The user's display name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The plain-text password (will be hashed).</param>
    /// <returns>A tuple indicating success, a message, and the created user if successful.</returns>
    Task<(bool Success, string Message, User? User)> RegisterAsync(string displayName, string email, string password);

    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The plain-text password.</param>
    /// <returns>A tuple indicating success, a message, and the user if authentication succeeded.</returns>
    Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password);

    /// <summary>
    /// Retrieves the currently logged-in user, if any.
    /// </summary>
    /// <returns>The current user, or null if not logged in.</returns>
    Task<User?> GetCurrentUserAsync();

    /// <summary>
    /// Logs out the current user by clearing stored credentials.
    /// </summary>
    Task LogoutAsync();
}
