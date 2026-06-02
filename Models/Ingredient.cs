using SQLite;

namespace FoodLens.Models;

/// <summary>
/// Represents a single ingredient with quantity and unit.
/// </summary>
public class Ingredient
{
    /// <summary>
    /// Unique identifier for the ingredient.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Name of the ingredient (e.g., "Chicken Breast").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Numeric quantity amount.
    /// </summary>
    public double Quantity { get; set; }

    /// <summary>
    /// Unit of measurement (e.g., "g", "ml", "whole", "tbsp").
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key referencing the parent recipe.
    /// </summary>
    public int RecipeId { get; set; }
}
