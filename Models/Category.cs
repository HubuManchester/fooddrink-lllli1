namespace FoodLens.Models;

/// <summary>
/// Represents a recipe category with display metadata.
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Display name of the category (e.g., "Breakfast", "Dinner").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Emoji icon used to represent the category.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Hex color string for the category's theme color.
    /// </summary>
    public string Color { get; set; } = "#512BD4";
}
