namespace FoodLens.Controls;

public partial class RatingStarView : ContentView
{
    public static readonly BindableProperty RatingProperty =
        BindableProperty.Create(nameof(Rating), typeof(double), typeof(RatingStarView), 0.0, propertyChanged: OnRatingChanged);

    public static readonly BindableProperty Star1VisibleProperty =
        BindableProperty.Create(nameof(Star1Visible), typeof(bool), typeof(RatingStarView), false);

    public static readonly BindableProperty Star2VisibleProperty =
        BindableProperty.Create(nameof(Star2Visible), typeof(bool), typeof(RatingStarView), false);

    public static readonly BindableProperty Star3VisibleProperty =
        BindableProperty.Create(nameof(Star3Visible), typeof(bool), typeof(RatingStarView), false);

    public static readonly BindableProperty Star4VisibleProperty =
        BindableProperty.Create(nameof(Star4Visible), typeof(bool), typeof(RatingStarView), false);

    public static readonly BindableProperty Star5VisibleProperty =
        BindableProperty.Create(nameof(Star5Visible), typeof(bool), typeof(RatingStarView), false);

    public double Rating
    {
        get => (double)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public bool Star1Visible
    {
        get => (bool)GetValue(Star1VisibleProperty);
        set => SetValue(Star1VisibleProperty, value);
    }

    public bool Star2Visible
    {
        get => (bool)GetValue(Star2VisibleProperty);
        set => SetValue(Star2VisibleProperty, value);
    }

    public bool Star3Visible
    {
        get => (bool)GetValue(Star3VisibleProperty);
        set => SetValue(Star3VisibleProperty, value);
    }

    public bool Star4Visible
    {
        get => (bool)GetValue(Star4VisibleProperty);
        set => SetValue(Star4VisibleProperty, value);
    }

    public bool Star5Visible
    {
        get => (bool)GetValue(Star5VisibleProperty);
        set => SetValue(Star5VisibleProperty, value);
    }

    public RatingStarView()
    {
        InitializeComponent();
    }

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
