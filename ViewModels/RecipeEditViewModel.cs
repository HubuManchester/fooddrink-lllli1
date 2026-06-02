using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the recipe creation and editing page, providing a form with fields for
/// recipe metadata, ingredients, and instructions, along with speech-to-text input support.
/// </summary>
public partial class RecipeEditViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDataService _dataService;
    private readonly ISpeechRecognitionService _speechRecognition;

    /// <summary>
    /// Gets or sets the unique identifier of the recipe being edited; zero for a new recipe.
    /// </summary>
    [ObservableProperty]
    private int recipeId;

    /// <summary>
    /// Gets or sets a value indicating whether the form is in edit mode (vs. create mode).
    /// </summary>
    [ObservableProperty]
    private bool isEditMode;

    /// <summary>
    /// Gets or sets the recipe title entered by the user.
    /// </summary>
    [ObservableProperty]
    private string title = string.Empty;

    /// <summary>
    /// Gets or sets the recipe description entered by the user.
    /// </summary>
    [ObservableProperty]
    private string description = string.Empty;

    /// <summary>
    /// Gets or sets the selected recipe category.
    /// </summary>
    [ObservableProperty]
    private string category = "Dinner";

    /// <summary>
    /// Gets or sets the cook time in minutes as a text string for input binding.
    /// </summary>
    [ObservableProperty]
    private string cookTimeMinutesText = "20";

    /// <summary>
    /// Gets or sets the selected difficulty level.
    /// </summary>
    [ObservableProperty]
    private string difficulty = "Medium";

    /// <summary>
    /// Gets or sets the calorie count as a text string for input binding.
    /// </summary>
    [ObservableProperty]
    private string caloriesText = "300";

    /// <summary>
    /// Gets or sets the URL of the recipe's image.
    /// </summary>
    [ObservableProperty]
    private string imageUrl = string.Empty;

    /// <summary>
    /// Gets or sets the number of servings as a text string for input binding.
    /// </summary>
    [ObservableProperty]
    private string servingsText = "2";

    /// <summary>
    /// Gets or sets the collection of ingredient items for the recipe.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<IngredientItem> ingredients = new();

    /// <summary>
    /// Gets or sets the collection of instruction steps for the recipe.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> instructions = new();

    /// <summary>
    /// Gets or sets the name of the new ingredient being entered.
    /// </summary>
    [ObservableProperty]
    private string newIngredientName = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the new ingredient being entered.
    /// </summary>
    [ObservableProperty]
    private string newIngredientQuantity = string.Empty;

    /// <summary>
    /// Gets or sets the unit of measurement for the new ingredient being entered.
    /// </summary>
    [ObservableProperty]
    private string newIngredientUnit = string.Empty;

    /// <summary>
    /// Gets or sets the text of the new instruction step being entered.
    /// </summary>
    [ObservableProperty]
    private string newInstruction = string.Empty;

    /// <summary>
    /// Gets or sets the validation error message displayed to the user.
    /// </summary>
    [ObservableProperty]
    private string errorMessage = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether a validation error is currently displayed.
    /// </summary>
    [ObservableProperty]
    private bool hasError;

    /// <summary>
    /// Gets or sets a value indicating whether speech recognition is currently listening.
    /// </summary>
    [ObservableProperty]
    private bool isListening;

    /// <summary>
    /// Gets the list of available category options for the recipe.
    /// </summary>
    public List<string> CategoryOptions { get; } = new() { "Breakfast", "Lunch", "Dinner", "Dessert", "Drinks" };

    /// <summary>
    /// Gets the list of available difficulty options for the recipe.
    /// </summary>
    public List<string> DifficultyOptions { get; } = new() { "Easy", "Medium", "Hard" };

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeEditViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for saving and loading recipe data.</param>
    /// <param name="speechRecognition">The speech recognition service for voice input.</param>
    public RecipeEditViewModel(IDataService dataService, ISpeechRecognitionService speechRecognition)
    {
        _dataService = dataService;
        _speechRecognition = speechRecognition;
        Title = "New Recipe";
    }

    /// <summary>
    /// Applies query attributes from navigation. If an "id" parameter is present, loads
    /// the recipe for editing; otherwise, sets up the form for creating a new recipe.
    /// </summary>
    /// <param name="query">The dictionary of navigation query parameters.</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && int.TryParse(id?.ToString(), out var rid))
        {
            RecipeId = rid;
            IsEditMode = true;
            ViewModelTitle = "Edit Recipe";
            _ = LoadRecipeAsync(rid);
        }
        else
        {
            IsEditMode = false;
            ViewModelTitle = "New Recipe";
        }
    }

    /// <summary>
    /// Gets or sets the page title displayed at the top of the form, reflecting create or edit mode.
    /// </summary>
    [ObservableProperty]
    private string viewModelTitle = "New Recipe";

    /// <summary>
    /// Loads an existing recipe's data into the form fields for editing.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to load.</param>
    [RelayCommand]
    private async Task LoadRecipeAsync(int id)
    {
        IsBusy = true;
        try
        {
            var recipe = await _dataService.GetRecipeByIdAsync(id);
            if (recipe is null) return;

            Title = recipe.Title;
            Description = recipe.Description;
            Category = recipe.Category;
            CookTimeMinutesText = recipe.CookTimeMinutes.ToString();
            Difficulty = recipe.Difficulty;
            CaloriesText = recipe.Calories.ToString();
            ImageUrl = recipe.ImageUrl;
            ServingsText = recipe.Servings.ToString();

            Ingredients = new ObservableCollection<IngredientItem>(
                recipe.Ingredients.Select(i => new IngredientItem
                {
                    Name = i.Name,
                    Quantity = i.Quantity.ToString(),
                    Unit = i.Unit
                }));

            Instructions = new ObservableCollection<string>(recipe.Instructions);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Adds a new ingredient to the ingredients list from the current input fields.
    /// </summary>
    [RelayCommand]
    private void AddIngredient()
    {
        if (string.IsNullOrWhiteSpace(NewIngredientName)) return;
        Ingredients.Add(new IngredientItem
        {
            Name = NewIngredientName,
            Quantity = string.IsNullOrWhiteSpace(NewIngredientQuantity) ? "1" : NewIngredientQuantity,
            Unit = string.IsNullOrWhiteSpace(NewIngredientUnit) ? "pcs" : NewIngredientUnit
        });
        NewIngredientName = string.Empty;
        NewIngredientQuantity = string.Empty;
        NewIngredientUnit = string.Empty;
    }

    /// <summary>
    /// Removes the specified ingredient item from the ingredients list.
    /// </summary>
    /// <param name="item">The ingredient item to remove.</param>
    [RelayCommand]
    private void RemoveIngredient(IngredientItem item)
    {
        Ingredients.Remove(item);
    }

    /// <summary>
    /// Adds a new instruction step to the instructions list from the current input field.
    /// </summary>
    [RelayCommand]
    private void AddInstruction()
    {
        if (string.IsNullOrWhiteSpace(NewInstruction)) return;
        Instructions.Add(NewInstruction);
        NewInstruction = string.Empty;
    }

    /// <summary>
    /// Removes the specified instruction step from the instructions list.
    /// </summary>
    /// <param name="instruction">The instruction text to remove.</param>
    [RelayCommand]
    private void RemoveInstruction(string instruction)
    {
        Instructions.Remove(instruction);
    }

    /// <summary>
    /// Validates all form fields and saves the recipe as a new entry or updates the existing one.
    /// </summary>
    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        HasError = false;
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Title))
        {
            ShowError("Title is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            ShowError("Description is required.");
            return;
        }

        if (!int.TryParse(CookTimeMinutesText, out var cookTime) || cookTime < 0)
        {
            ShowError("Cook time must be a non-negative integer.");
            return;
        }

        if (!int.TryParse(CaloriesText, out var calories) || calories < 0)
        {
            ShowError("Calories must be a non-negative integer.");
            return;
        }

        if (!int.TryParse(ServingsText, out var servings) || servings < 1)
        {
            ShowError("Servings must be at least 1.");
            return;
        }

        var validIngredients = Ingredients
            .Where(i => !string.IsNullOrWhiteSpace(i.Name))
            .Select(i => new Ingredient
            {
                Name = i.Name,
                Quantity = double.TryParse(i.Quantity, out var q) ? q : 1,
                Unit = i.Unit
            }).ToList();

        if (validIngredients.Count == 0)
        {
            ShowError("At least one ingredient is required.");
            return;
        }

        var validInstructions = Instructions
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        if (validInstructions.Count == 0)
        {
            ShowError("At least one instruction step is required.");
            return;
        }

        IsBusy = true;
        try
        {
            var recipe = new Recipe
            {
                Title = Title.Trim(),
                Description = Description.Trim(),
                Category = Category,
                CookTimeMinutes = cookTime,
                Difficulty = Difficulty,
                Calories = calories,
                ImageUrl = ImageUrl.Trim(),
                Servings = servings,
                Ingredients = validIngredients,
                Instructions = validInstructions,
                Rating = 4.0
            };

            if (IsEditMode)
            {
                recipe.Id = RecipeId;
                await _dataService.UpdateRecipeAsync(recipe);
            }
            else
            {
                await _dataService.AddRecipeAsync(recipe);
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ShowError($"Save failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Prompts the user for confirmation and permanently deletes the recipe being edited.
    /// </summary>
    [RelayCommand]
    private async Task DeleteRecipeAsync()
    {
        if (!IsEditMode) return;

        var confirm = await Shell.Current.DisplayAlert(
            "Delete Recipe",
            "Are you sure you want to delete this recipe? This action cannot be undone.",
            "Delete", "Cancel");

        if (!confirm) return;

        IsBusy = true;
        try
        {
            await _dataService.DeleteRecipeAsync(RecipeId);
            await Shell.Current.GoToAsync("../..");
        }
        catch (Exception ex)
        {
            ShowError($"Delete failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Starts speech recognition and assigns the recognized text to the specified target field.
    /// </summary>
    /// <param name="targetField">
    /// The field to populate with recognized speech. Valid values: "title", "description", "ingredient", "instruction".
    /// </param>
    [RelayCommand]
    private async Task StartSpeechAsync(string targetField)
    {
        if (!_speechRecognition.IsSupported)
        {
            await Shell.Current.DisplayAlert("Not Supported", "Speech recognition is not available on this device.", "OK");
            return;
        }

        if (IsListening) return;
        IsListening = true;

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var result = await _speechRecognition.ListenAsync(cts.Token);

            if (string.IsNullOrWhiteSpace(result))
            {
                await Shell.Current.DisplayAlert("Speech Recognition", "No speech was recognized. Please try again.", "OK");
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (targetField)
                {
                    case "title":
                        Title = result;
                        break;
                    case "description":
                        Description = result;
                        break;
                    case "ingredient":
                        NewIngredientName = result;
                        break;
                    case "instruction":
                        NewInstruction = result;
                        break;
                }
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Speech Error", $"Recognition failed: {ex.Message}", "OK");
        }
        finally
        {
            IsListening = false;
        }
    }

    /// <summary>
    /// Navigates back to the previous page without saving.
    /// </summary>
    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Displays a validation error message to the user.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = true;
    }
}

/// <summary>
/// Represents a single ingredient entry in the recipe edit form with string-based quantity for input binding.
/// </summary>
public class IngredientItem
{
    /// <summary>
    /// Gets or sets the name of the ingredient.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the ingredient as a string for flexible input.
    /// </summary>
    public string Quantity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unit of measurement for the ingredient.
    /// </summary>
    public string Unit { get; set; } = string.Empty;
}
