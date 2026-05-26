namespace FoodLens.Models;

public class Recipe
{
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
    public List<string> Instructions { get; set; } = new();
    public List<Ingredient> Ingredients { get; set; } = new();
}
