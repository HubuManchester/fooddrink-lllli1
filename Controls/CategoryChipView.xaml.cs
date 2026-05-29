using System.Windows.Input;

namespace FoodLens.Controls;

public partial class CategoryChipView : ContentView
{
    public static readonly BindableProperty CategoryNameProperty =
        BindableProperty.Create(nameof(CategoryName), typeof(string), typeof(CategoryChipView), string.Empty);

    public static readonly BindableProperty ChipIconProperty =
        BindableProperty.Create(nameof(ChipIcon), typeof(string), typeof(CategoryChipView), string.Empty);

    public static readonly BindableProperty ChipColorProperty =
        BindableProperty.Create(nameof(ChipColor), typeof(string), typeof(CategoryChipView), "#808080");

    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(CategoryChipView));

    public string CategoryName
    {
        get => (string)GetValue(CategoryNameProperty);
        set => SetValue(CategoryNameProperty, value);
    }

    public string ChipIcon
    {
        get => (string)GetValue(ChipIconProperty);
        set => SetValue(ChipIconProperty, value);
    }

    public string ChipColor
    {
        get => (string)GetValue(ChipColorProperty);
        set => SetValue(ChipColorProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public CategoryChipView()
    {
        InitializeComponent();
    }
}
