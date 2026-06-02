namespace FoodLens.Controls;

/// <summary>
/// A reusable star rating display control that renders 1 to 5 filled stars based on a numeric rating value.
/// </summary>
public partial class RatingStarView : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Rating"/> bindable property.
    /// </summary>
    public static readonly BindableProperty RatingProperty =
        BindableProperty.Create(nameof(Rating), typeof(double), typeof(RatingStarView), 0.0, propertyChanged: OnRatingChanged);

    /// <summary>
    /// Identifies the <see cref="Star1Visible"/> bindable property.
    /// </summary>
    public static readonly BindableProperty Star1VisibleProperty =
        BindableProperty.Create(nameof(Star1Visible), typeof(bool), typeof(RatingStarView), false);

    /// <summary>
    /// Identifies the <see cref="Star2Visible"/> bindable property.
    /// </summary>
    public static readonly BindableProperty Star2VisibleProperty =
        BindableProperty.Create(nameof(Star2Visible), typeof(bool), typeof(RatingStarView), false);

    /// <summary>
    /// Identifies the <see cref="Star3Visible"/> bindable property.
    /// </summary>
    public static readonly BindableProperty Star3VisibleProperty =
        BindableProperty.Create(nameof(Star3Visible), typeof(bool), typeof(RatingStarView), false);

    /// <summary>
    /// Identifies the <see cref="Star4Visible"/> bindable property.
    /// </summary>
    public static readonly BindableProperty Star4VisibleProperty =
        BindableProperty.Create(nameof(Star4Visible), typeof(bool), typeof(RatingStarView), false);

    /// <summary>
    /// Identifies the <see cref="Star5Visible"/> bindable property.
    /// </summary>
    public static readonly BindableProperty Star5VisibleProperty =
        BindableProperty.Create(nameof(Star5Visible), typeof(bool), typeof(RatingStarView), false);

    /// <summary>
    /// Gets or sets the numeric rating value (0.0 to 5.0) that determines how many stars are visible.
    /// </summary>
    public double Rating
    {
        get => (double)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the first star is visible.
    /// </summary>
    public bool Star1Visible
    {
        get => (bool)GetValue(Star1VisibleProperty);
        set => SetValue(Star1VisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the second star is visible.
    /// </summary>
    public bool Star2Visible
    {
        get => (bool)GetValue(Star2VisibleProperty);
        set => SetValue(Star2VisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the third star is visible.
    /// </summary>
    public bool Star3Visible
    {
        get => (bool)GetValue(Star3VisibleProperty);
        set => SetValue(Star3VisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the fourth star is visible.
    /// </summary>
    public bool Star4Visible
    {
        get => (bool)GetValue(Star4VisibleProperty);
        set => SetValue(Star4VisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the fifth star is visible.
    /// </summary>
    public bool Star5Visible
    {
        get => (bool)GetValue(Star5VisibleProperty);
        set => SetValue(Star5VisibleProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RatingStarView"/> class.
    /// </summary>
    public RatingStarView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles changes to the <see cref="Rating"/> property by updating the visibility of each star.
    /// </summary>
    /// <param name="bindable">The bindable object that raised the change.</param>
    /// <param name="oldValue">The previous value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnRatingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RatingStarView)bindable;
        var rating = (double)newValue;
        control.Star1Visible = rating >= 1.0;
        control.Star2Visible = rating >= 2.0;
        control.Star3Visible = rating >= 3.0;
        control.Star4Visible = rating >= 4.0;
        control.Star5Visible = rating >= 5.0;
    }
}
