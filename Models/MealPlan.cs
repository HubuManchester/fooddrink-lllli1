using SQLite;

namespace FoodLens.Models;

/// <summary>
/// Represents a meal plan entry linking a recipe to a specific date and meal type.
/// </summary>
public class MealPlan
{
    /// <summary>
    /// Unique identifier for the meal plan entry.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// The date this meal is planned for.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Type of meal (e.g., Breakfast, Lunch, Dinner, Snack).
    /// </summary>
    public string MealType { get; set; } = "Lunch";

    /// <summary>
    /// Foreign key referencing the associated recipe.
    /// </summary>
    public int RecipeId { get; set; }

    /// <summary>
    /// Navigation property to the associated recipe (not stored in database).
    /// </summary>
    [Ignore]
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Display title of the associated recipe, or "Unknown Recipe" if not loaded.
    /// </summary>
    [Ignore]
    public string RecipeTitle => Recipe?.Title ?? "Unknown Recipe";
}
