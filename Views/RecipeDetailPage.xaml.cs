using System.Diagnostics;
using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class RecipeDetailPage : ContentPage, IQueryAttributable
{
    private readonly RecipeDetailViewModel _viewModel;

    public RecipeDetailPage(RecipeDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        ApplyLayout(PlatformHelper.IsWideLayout);
        PlatformHelper.LayoutChanged += OnLayoutChanged;
    }

    private void OnLayoutChanged(object? sender, LayoutModeChangedEventArgs e)
    {
        ApplyLayout(e.IsWideLayout);
    }

    private void ApplyLayout(bool wide)
    {
        if (wide)
        {
            HeroImageGrid.HeightRequest = 500;
            HeroImageGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                new ColumnDefinition(new GridLength(2, GridUnitType.Star)),
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            };
            HeroImage.HeightRequest = 500;
        }
        else
        {
            HeroImageGrid.HeightRequest = 250;
            HeroImageGrid.ColumnDefinitions.Clear();
            HeroImage.HeightRequest = 250;
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        foreach (var kv in query)
            Debug.WriteLine($"[RecipeDetailPage] QueryParam: {kv.Key} = {kv.Value} ({kv.Value?.GetType().Name})");

        (_viewModel as IQueryAttributable)?.ApplyQueryAttributes(query);
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        PlatformHelper.LayoutChanged -= OnLayoutChanged;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PlatformHelper.LayoutChanged -= OnLayoutChanged;
        PlatformHelper.LayoutChanged += OnLayoutChanged;
        Debug.WriteLine($"[RecipeDetailPage] OnNavigatedTo, Recipe={_viewModel.Recipe?.Title ?? "null"}");
    }
}
