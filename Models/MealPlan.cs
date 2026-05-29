using SQLite;

namespace FoodLens.Models;

public class MealPlan
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string MealType { get; set; } = "Lunch";
    public int RecipeId { get; set; }

    [Ignore]
    public Recipe? Recipe { get; set; }

    [Ignore]
    public string RecipeTitle => Recipe?.Title ?? "Unknown Recipe";
}
