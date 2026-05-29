using System.Windows.Input;
using FoodLens.Models;

namespace FoodLens.Controls;

public partial class RecipeCardView : ContentView
{
    public static readonly BindableProperty RecipeIdProperty =
        BindableProperty.Create(nameof(RecipeId), typeof(int), typeof(RecipeCardView), 0);

    public static readonly BindableProperty RecipeTitleProperty =
        BindableProperty.Create(nameof(RecipeTitle), typeof(string), typeof(RecipeCardView), string.Empty);

    public static readonly BindableProperty RecipeDescriptionProperty =
        BindableProperty.Create(nameof(RecipeDescription), typeof(string), typeof(RecipeCardView), string.Empty);

    public static readonly BindableProperty CookTimeProperty =
        BindableProperty.Create(nameof(CookTime), typeof(int), typeof(RecipeCardView), 0);

    public static readonly BindableProperty DifficultyProperty =
        BindableProperty.Create(nameof(Difficulty), typeof(string), typeof(RecipeCardView), "Easy");

    public static readonly BindableProperty CaloriesProperty =
        BindableProperty.Create(nameof(Calories), typeof(int), typeof(RecipeCardView), 0);

    public static readonly BindableProperty CategoryProperty =
        BindableProperty.Create(nameof(Category), typeof(string), typeof(RecipeCardView), string.Empty);

    public static readonly BindableProperty IsFavoriteProperty =
        BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(RecipeCardView), false, propertyChanged: OnIsFavoriteChanged);

    public static readonly BindableProperty FavoriteIconProperty =
        BindableProperty.Create(nameof(FavoriteIcon), typeof(string), typeof(RecipeCardView), "heart_outline.png");

    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(RecipeCardView));

    public static readonly BindableProperty FavoriteCommandProperty =
        BindableProperty.Create(nameof(FavoriteCommand), typeof(ICommand), typeof(RecipeCardView));

    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(RecipeCardView), string.Empty);

    public int RecipeId
    {
        get => (int)GetValue(RecipeIdProperty);
        set => SetValue(RecipeIdProperty, value);
    }

    public string RecipeTitle
    {
        get => (string)GetValue(RecipeTitleProperty);
        set => SetValue(RecipeTitleProperty, value);
    }

    public string RecipeDescription
    {
        get => (string)GetValue(RecipeDescriptionProperty);
        set => SetValue(RecipeDescriptionProperty, value);
    }

    public int CookTime
    {
        get => (int)GetValue(CookTimeProperty);
        set => SetValue(CookTimeProperty, value);
    }

    public string Difficulty
    {
        get => (string)GetValue(DifficultyProperty);
        set => SetValue(DifficultyProperty, value);
    }

    public int Calories
    {
        get => (int)GetValue(CaloriesProperty);
        set => SetValue(CaloriesProperty, value);
    }

    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    public string FavoriteIcon
    {
        get => (string)GetValue(FavoriteIconProperty);
        set => SetValue(FavoriteIconProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public ICommand FavoriteCommand
    {
        get => (ICommand)GetValue(FavoriteCommandProperty);
        set => SetValue(FavoriteCommandProperty, value);
    }

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public RecipeCardView()
    {
        InitializeComponent();
    }

    private static void OnIsFavoriteChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeCardView)bindable;
        control.FavoriteIcon = (bool)newValue ? "heart_filled.png" : "heart_outline.png";
    }
}
