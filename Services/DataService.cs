using FoodLens.Models;
using SQLite;

namespace FoodLens.Services;

public class DataService : IDataService
{
    private SQLiteAsyncConnection? _database;
    private List<Recipe> _seedRecipes = new();
    private List<Category> _categories = new();

    public async Task InitializeAsync()
    {
        if (_database is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "foodlens.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Recipe>();
        await _database.CreateTableAsync<Ingredient>();
        await _database.CreateTableAsync<MealPlan>();
        await _database.CreateTableAsync<User>();

        await LoadSeedDataAsync();
    }

    private async Task LoadSeedDataAsync()
    {
        var existingCount = await _database!.Table<Recipe>().CountAsync();
        if (existingCount > 0)
        {
            _seedRecipes = await _database.Table<Recipe>().ToListAsync();
            return;
        }

        _seedRecipes = new List<Recipe>
        {
            new()
            {
                Id = 1, Title = "Classic Pasta Carbonara",
                Description = "A creamy Italian pasta dish made with eggs, cheese, pancetta, and black pepper.",
                Category = "Dinner", CookTimeMinutes = 20, PrepTimeMinutes = 10, Servings = 4,
                Difficulty = "Medium", Rating = 4.8, Calories = 450,
                Instructions = new() { "Bring a large pot of salted water to boil and cook spaghetti until al dente.", "While pasta cooks, dice the pancetta and fry in a pan until crispy.", "In a bowl, whisk together eggs, grated Pecorino Romano, and black pepper.", "Drain pasta, reserving 1 cup of pasta water.", "Add pasta to the pancetta pan, remove from heat, and pour in egg mixture.", "Toss quickly, adding pasta water as needed for a creamy consistency.", "Serve immediately with extra cheese and pepper." },
                Ingredients = new() { new() { Id = 1, Name = "Spaghetti", Quantity = 400, Unit = "g" }, new() { Id = 2, Name = "Pancetta", Quantity = 200, Unit = "g" }, new() { Id = 3, Name = "Eggs", Quantity = 4, Unit = "whole" }, new() { Id = 4, Name = "Pecorino Romano", Quantity = 100, Unit = "g" }, new() { Id = 5, Name = "Black Pepper", Quantity = 2, Unit = "tsp" } }
            },
            new()
            {
                Id = 2, Title = "Avocado Toast with Poached Egg",
                Description = "A nutritious breakfast with creamy avocado, perfectly poached egg on sourdough bread.",
                Category = "Breakfast", CookTimeMinutes = 10, PrepTimeMinutes = 5, Servings = 2,
                Difficulty = "Easy", Rating = 4.5, Calories = 320,
                Instructions = new() { "Toast the sourdough bread until golden brown.", "Halve the avocado, remove pit, and mash with a fork.", "Season avocado with salt, pepper, and lemon juice.", "Bring water to a gentle simmer, add a splash of vinegar.", "Create a whirlpool and gently drop in the egg.", "Poach for 3-4 minutes until whites are set.", "Spread avocado on toast, top with poached egg.", "Season with salt, pepper, and red pepper flakes." },
                Ingredients = new() { new() { Id = 6, Name = "Sourdough Bread", Quantity = 2, Unit = "slices" }, new() { Id = 7, Name = "Avocado", Quantity = 1, Unit = "whole" }, new() { Id = 8, Name = "Eggs", Quantity = 2, Unit = "whole" }, new() { Id = 9, Name = "Lemon Juice", Quantity = 1, Unit = "tbsp" }, new() { Id = 10, Name = "Red Pepper Flakes", Quantity = 1, Unit = "pinch" } }
            },
            new()
            {
                Id = 3, Title = "Chicken Stir Fry",
                Description = "A quick and healthy stir fry with tender chicken and fresh vegetables.",
                Category = "Lunch", CookTimeMinutes = 15, PrepTimeMinutes = 10, Servings = 3,
                Difficulty = "Easy", Rating = 4.6, Calories = 380, IsFavorite = true,
                Instructions = new() { "Slice chicken breast into thin strips.", "Prepare vegetables: slice bell peppers, broccoli, and carrots.", "Heat oil in a wok over high heat.", "Stir fry chicken until golden, about 5 minutes.", "Remove chicken, add vegetables and stir fry for 3 minutes.", "Return chicken to wok, add soy sauce and oyster sauce.", "Toss everything together for 2 minutes.", "Serve over steamed rice." },
                Ingredients = new() { new() { Id = 11, Name = "Chicken Breast", Quantity = 500, Unit = "g" }, new() { Id = 12, Name = "Bell Peppers", Quantity = 2, Unit = "whole" }, new() { Id = 13, Name = "Broccoli", Quantity = 200, Unit = "g" }, new() { Id = 14, Name = "Carrots", Quantity = 2, Unit = "whole" }, new() { Id = 15, Name = "Soy Sauce", Quantity = 3, Unit = "tbsp" }, new() { Id = 16, Name = "Oyster Sauce", Quantity = 2, Unit = "tbsp" } }
            },
            new()
            {
                Id = 4, Title = "Chocolate Lava Cake",
                Description = "A decadent dessert with a molten chocolate center that flows when you cut into it.",
                Category = "Dessert", CookTimeMinutes = 14, PrepTimeMinutes = 15, Servings = 4,
                Difficulty = "Medium", Rating = 4.9, Calories = 520,
                Instructions = new() { "Preheat oven to 220°C (425°F). Butter and flour 4 ramekins.", "Melt butter and chocolate together in a double boiler.", "Whisk in sugar and a pinch of salt.", "Add eggs one at a time, whisking well.", "Fold in flour until just combined.", "Divide batter among ramekins.", "Bake for 12-14 minutes until edges are firm but center is soft.", "Let cool 1 minute, then invert onto plates and serve immediately." },
                Ingredients = new() { new() { Id = 17, Name = "Dark Chocolate", Quantity = 200, Unit = "g" }, new() { Id = 18, Name = "Butter", Quantity = 100, Unit = "g" }, new() { Id = 19, Name = "Sugar", Quantity = 100, Unit = "g" }, new() { Id = 20, Name = "Eggs", Quantity = 2, Unit = "whole" }, new() { Id = 21, Name = "Flour", Quantity = 50, Unit = "g" } }
            },
            new()
            {
                Id = 5, Title = "Fresh Berry Smoothie",
                Description = "A refreshing and healthy smoothie packed with mixed berries and yogurt.",
                Category = "Drinks", CookTimeMinutes = 0, PrepTimeMinutes = 5, Servings = 2,
                Difficulty = "Easy", Rating = 4.3, Calories = 180,
                Instructions = new() { "Add frozen mixed berries to the blender.", "Add Greek yogurt and a splash of milk.", "Add honey for sweetness.", "Blend on high until smooth and creamy.", "Pour into glasses and serve immediately." },
                Ingredients = new() { new() { Id = 22, Name = "Mixed Berries (frozen)", Quantity = 300, Unit = "g" }, new() { Id = 23, Name = "Greek Yogurt", Quantity = 200, Unit = "g" }, new() { Id = 24, Name = "Milk", Quantity = 100, Unit = "ml" }, new() { Id = 25, Name = "Honey", Quantity = 2, Unit = "tbsp" } }
            },
            new()
            {
                Id = 6, Title = "Grilled Salmon with Lemon",
                Description = "Perfectly grilled salmon fillet with a zesty lemon herb butter.",
                Category = "Dinner", CookTimeMinutes = 15, PrepTimeMinutes = 10, Servings = 2,
                Difficulty = "Medium", Rating = 4.7, Calories = 420, IsFavorite = true,
                Instructions = new() { "Season salmon fillets with salt and pepper.", "Mix softened butter with lemon juice, garlic, and herbs.", "Preheat grill to medium-high heat.", "Place salmon skin-side down on the grill.", "Cook for 6-7 minutes per side.", "Top with lemon herb butter before serving.", "Serve with steamed asparagus and rice." },
                Ingredients = new() { new() { Id = 26, Name = "Salmon Fillets", Quantity = 2, Unit = "whole" }, new() { Id = 27, Name = "Butter", Quantity = 50, Unit = "g" }, new() { Id = 28, Name = "Lemon", Quantity = 1, Unit = "whole" }, new() { Id = 29, Name = "Garlic", Quantity = 2, Unit = "cloves" }, new() { Id = 30, Name = "Fresh Dill", Quantity = 1, Unit = "tbsp" } }
            }
        };

        foreach (var recipe in _seedRecipes)
        {
            await _database.InsertAsync(recipe);
        }

        _categories = new List<Category>
        {
            new() { Id = 1, Name = "Breakfast", Icon = "\U0001F373", Color = "#FF9800" },
            new() { Id = 2, Name = "Lunch", Icon = "\U0001F957", Color = "#4CAF50" },
            new() { Id = 3, Name = "Dinner", Icon = "\U0001F356", Color = "#F44336" },
            new() { Id = 4, Name = "Dessert", Icon = "\U0001F370", Color = "#E91E63" },
            new() { Id = 5, Name = "Drinks", Icon = "\U0001F379", Color = "#2196F3" }
        };
    }

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        await InitializeAsync();
        return await _database!.Table<Recipe>().ToListAsync();
    }

    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        await InitializeAsync();
        return await _database!.Table<Recipe>().FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Recipe>> SearchRecipesAsync(string keyword, string? category = null)
    {
        await InitializeAsync();
        var recipes = await _database!.Table<Recipe>().ToListAsync();

        if (!string.IsNullOrEmpty(category))
            recipes = recipes.Where(r => r.Category == category).ToList();

        if (!string.IsNullOrWhiteSpace(keyword))
            recipes = recipes.Where(r => r.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || r.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        return recipes;
    }

    public async Task<List<Recipe>> GetFavoriteRecipesAsync()
    {
        await InitializeAsync();
        return await _database!.Table<Recipe>().Where(r => r.IsFavorite).ToListAsync();
    }

    public async Task ToggleFavoriteAsync(int recipeId)
    {
        await InitializeAsync();
        var recipe = await _database!.Table<Recipe>().FirstOrDefaultAsync(r => r.Id == recipeId);
        if (recipe is not null)
        {
            recipe.IsFavorite = !recipe.IsFavorite;
            await _database.UpdateAsync(recipe);
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        await InitializeAsync();
        return _categories;
    }

    public async Task AddMealPlanAsync(MealPlan mealPlan)
    {
        await InitializeAsync();
        await _database!.InsertAsync(mealPlan);
    }

    public async Task<List<MealPlan>> GetMealPlansAsync(DateTime date)
    {
        await InitializeAsync();
        var plans = await _database!.Table<MealPlan>().Where(m => m.Date.Date == date.Date).ToListAsync();
        foreach (var plan in plans)
        {
            plan.Recipe = await _database.Table<Recipe>().FirstOrDefaultAsync(r => r.Id == plan.RecipeId);
        }
        return plans;
    }

    public async Task RemoveMealPlanAsync(int id)
    {
        await InitializeAsync();
        await _database!.DeleteAsync<MealPlan>(id);
    }
}
