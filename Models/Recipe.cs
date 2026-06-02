using SQLite;
using System.Text.Json;

namespace FoodLens.Models;

/// <summary>
/// Represents a food recipe with ingredients, instructions, and metadata.
/// </summary>
public class Recipe
{
    /// <summary>
    /// Unique identifier for the recipe.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Display name of the recipe.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Short description of the recipe.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// URL of the recipe's cover image.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Category name (e.g., Breakfast, Lunch, Dinner, Dessert, Drinks).
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Cooking time in minutes.
    /// </summary>
    public int CookTimeMinutes { get; set; }

    /// <summary>
    /// Preparation time in minutes.
    /// </summary>
    public int PrepTimeMinutes { get; set; }

    /// <summary>
    /// Total time (preparation + cooking) in minutes.
    /// </summary>
    [Ignore]
    public int TotalTimeMinutes => PrepTimeMinutes + CookTimeMinutes;

    /// <summary>
    /// Number of servings the recipe yields.
    /// </summary>
    public int Servings { get; set; }

    /// <summary>
    /// Difficulty level (Easy, Medium, or Hard).
    /// </summary>
    public string Difficulty { get; set; } = "Easy";

    /// <summary>
    /// User rating from 0.0 to 5.0.
    /// </summary>
    public double Rating { get; set; }

    /// <summary>
    /// Estimated calories per serving.
    /// </summary>
    public int Calories { get; set; }

    /// <summary>
    /// Whether the recipe is marked as a favorite.
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    /// Timestamp when the recipe was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// JSON-serialized list of cooking instruction steps.
    /// </summary>
    public string InstructionsJson { get; set; } = "[]";

    /// <summary>
    /// Deserialized list of cooking instruction steps.
    /// </summary>
    [Ignore]
    public List<string> Instructions
    {
        get => JsonSerializer.Deserialize<List<string>>(InstructionsJson) ?? new();
        set => InstructionsJson = JsonSerializer.Serialize(value);
    }

    /// <summary>
    /// JSON-serialized list of ingredients.
    /// </summary>
    public string IngredientsJson { get; set; } = "[]";

    /// <summary>
    /// Deserialized list of ingredients.
    /// </summary>
    [Ignore]
    public List<Ingredient> Ingredients
    {
        get => JsonSerializer.Deserialize<List<Ingredient>>(IngredientsJson) ?? new();
        set => IngredientsJson = JsonSerializer.Serialize(value);
    }
}
