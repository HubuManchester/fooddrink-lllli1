using SQLite;
using System.Text.Json;

namespace FoodLens.Models;

public class Recipe
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CookTimeMinutes { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string Difficulty { get; set; } = "Easy";
    public double Rating { get; set; }
    public int Calories { get; set; }
    public bool IsFavorite { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string InstructionsJson { get; set; } = "[]";

    [Ignore]
    public List<string> Instructions
    {
        get => JsonSerializer.Deserialize<List<string>>(InstructionsJson) ?? new();
        set => InstructionsJson = JsonSerializer.Serialize(value);
    }

    public string IngredientsJson { get; set; } = "[]";

    [Ignore]
    public List<Ingredient> Ingredients
    {
        get => JsonSerializer.Deserialize<List<Ingredient>>(IngredientsJson) ?? new();
        set => IngredientsJson = JsonSerializer.Serialize(value);
    }
}
