using System.Windows.Input;
using FoodLens.Models;

namespace FoodLens.Controls;

/// <summary>
/// A reusable card view control that displays a recipe summary including title, image,
/// cook time, difficulty, calories, category, and favorite status with tap interaction.
/// </summary>
public partial class RecipeCardView : ContentView
{
    /// <summary>
    /// Identifies the <see cref="RecipeId"/> bindable property.
    /// </summary>
    public static readonly BindableProperty RecipeIdProperty =
        BindableProperty.Create(nameof(RecipeId), typeof(int), typeof(RecipeCardView), 0);

    /// <summary>
    /// Identifies the <see cref="RecipeTitle"/> bindable property.
    /// </summary>
    public static readonly BindableProperty RecipeTitleProperty =
        BindableProperty.Create(nameof(RecipeTitle), typeof(string), typeof(RecipeCardView), string.Empty);

    /// <summary>
    /// Identifies the <see cref="RecipeDescription"/> bindable property.
    /// </summary>
    public static readonly BindableProperty RecipeDescriptionProperty =
        BindableProperty.Create(nameof(RecipeDescription), typeof(string), typeof(RecipeCardView), string.Empty);

    /// <summary>
    /// Identifies the <see cref="CookTime"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CookTimeProperty =
        BindableProperty.Create(nameof(CookTime), typeof(int), typeof(RecipeCardView), 0);

    /// <summary>
    /// Identifies the <see cref="Difficulty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty DifficultyProperty =
        BindableProperty.Create(nameof(Difficulty), typeof(string), typeof(RecipeCardView), "Easy");

    /// <summary>
    /// Identifies the <see cref="Calories"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CaloriesProperty =
        BindableProperty.Create(nameof(Calories), typeof(int), typeof(RecipeCardView), 0);

    /// <summary>
    /// Identifies the <see cref="Category"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CategoryProperty =
        BindableProperty.Create(nameof(Category), typeof(string), typeof(RecipeCardView), string.Empty);

    /// <summary>
    /// Identifies the <see cref="IsFavorite"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsFavoriteProperty =
        BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(RecipeCardView), false, propertyChanged: OnIsFavoriteChanged);

    /// <summary>
    /// Identifies the <see cref="FavoriteIcon"/> bindable property.
    /// </summary>
    public static readonly BindableProperty FavoriteIconProperty =
        BindableProperty.Create(nameof(FavoriteIcon), typeof(string), typeof(RecipeCardView), "heart_outline.svg");

    /// <summary>
    /// Identifies the <see cref="TapCommand"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(RecipeCardView));

    /// <summary>
    /// Identifies the <see cref="FavoriteCommand"/> bindable property.
    /// </summary>
    public static readonly BindableProperty FavoriteCommandProperty =
        BindableProperty.Create(nameof(FavoriteCommand), typeof(ICommand), typeof(RecipeCardView));

    /// <summary>
    /// Identifies the <see cref="ImageUrl"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(RecipeCardView), string.Empty);

    /// <summary>
    /// Gets or sets the unique identifier of the recipe displayed in this card.
    /// </summary>
    public int RecipeId
    {
        get => (int)GetValue(RecipeIdProperty);
        set => SetValue(RecipeIdProperty, value);
    }

    /// <summary>
    /// Gets or sets the title of the recipe displayed in this card.
    /// </summary>
    public string RecipeTitle
    {
        get => (string)GetValue(RecipeTitleProperty);
        set => SetValue(RecipeTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the description of the recipe displayed in this card.
    /// </summary>
    public string RecipeDescription
    {
        get => (string)GetValue(RecipeDescriptionProperty);
        set => SetValue(RecipeDescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets the cook time in minutes for the recipe displayed in this card.
    /// </summary>
    public int CookTime
    {
        get => (int)GetValue(CookTimeProperty);
        set => SetValue(CookTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the difficulty level of the recipe displayed in this card.
    /// </summary>
    public string Difficulty
    {
        get => (string)GetValue(DifficultyProperty);
        set => SetValue(DifficultyProperty, value);
    }

    /// <summary>
    /// Gets or sets the calorie count of the recipe displayed in this card.
    /// </summary>
    public int Calories
    {
        get => (int)GetValue(CaloriesProperty);
        set => SetValue(CaloriesProperty, value);
    }

    /// <summary>
    /// Gets or sets the category name of the recipe displayed in this card.
    /// </summary>
    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the recipe is marked as a favorite.
    /// </summary>
    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon filename used to represent the favorite state.
    /// </summary>
    public string FavoriteIcon
    {
        get => (string)GetValue(FavoriteIconProperty);
        set => SetValue(FavoriteIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the card is tapped.
    /// </summary>
    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the favorite button is tapped.
    /// </summary>
    public ICommand FavoriteCommand
    {
        get => (ICommand)GetValue(FavoriteCommandProperty);
        set => SetValue(FavoriteCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the URL of the recipe image displayed in this card.
    /// </summary>
    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeCardView"/> class.
    /// </summary>
    public RecipeCardView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles changes to the <see cref="IsFavorite"/> property by updating the favorite icon.
    /// </summary>
    /// <param name="bindable">The bindable object that raised the change.</param>
    /// <param name="oldValue">The previous value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnIsFavoriteChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeCardView)bindable;
        control.FavoriteIcon = (bool)newValue ? "heart_filled.svg" : "heart_outline.svg";
    }
}
