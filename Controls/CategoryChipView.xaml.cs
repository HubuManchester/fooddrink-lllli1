using System.Windows.Input;

namespace FoodLens.Controls;

/// <summary>
/// A reusable chip control that displays a recipe category with a name, icon, and color accent.
/// </summary>
public partial class CategoryChipView : ContentView
{
    /// <summary>
    /// Identifies the <see cref="CategoryName"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CategoryNameProperty =
        BindableProperty.Create(nameof(CategoryName), typeof(string), typeof(CategoryChipView), string.Empty);

    /// <summary>
    /// Identifies the <see cref="ChipIcon"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ChipIconProperty =
        BindableProperty.Create(nameof(ChipIcon), typeof(string), typeof(CategoryChipView), string.Empty);

    /// <summary>
    /// Identifies the <see cref="ChipColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ChipColorProperty =
        BindableProperty.Create(nameof(ChipColor), typeof(string), typeof(CategoryChipView), "#808080");

    /// <summary>
    /// Identifies the <see cref="TapCommand"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(CategoryChipView));

    /// <summary>
    /// Gets or sets the name of the category displayed on this chip.
    /// </summary>
    public string CategoryName
    {
        get => (string)GetValue(CategoryNameProperty);
        set => SetValue(CategoryNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon displayed on this category chip.
    /// </summary>
    public string ChipIcon
    {
        get => (string)GetValue(ChipIconProperty);
        set => SetValue(ChipIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the accent color of this category chip in hex format.
    /// </summary>
    public string ChipColor
    {
        get => (string)GetValue(ChipColorProperty);
        set => SetValue(ChipColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the chip is tapped.
    /// </summary>
    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryChipView"/> class.
    /// </summary>
    public CategoryChipView()
    {
        InitializeComponent();
    }
}
