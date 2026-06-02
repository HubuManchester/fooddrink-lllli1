using System.Diagnostics;
using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the recipe detail page that displays full information about a recipe,
/// with platform-specific image sizing adjustments.
/// </summary>
public partial class RecipeDetailPage : ContentPage, IQueryAttributable
{
    private readonly RecipeDetailViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeDetailPage"/> class with the specified view model.
    /// Configures platform-specific hero image sizing for non-WinUI platforms.
    /// </summary>
    /// <param name="viewModel">The recipe detail view model that provides recipe data.</param>
    public RecipeDetailPage(RecipeDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        if (!PlatformHelper.IsTabletOrDesktop)
        {
            HeroImageGrid.HeightRequest = 250;
            HeroImageGrid.ColumnDefinitions.Clear();
            HeroImage.HeightRequest = 250;
        }
    }

    /// <summary>
    /// Applies query attributes received during navigation, forwarding them to the view model.
    /// </summary>
    /// <param name="query">A dictionary of navigation query parameters.</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        foreach (var kv in query)
            Debug.WriteLine($"[RecipeDetailPage] QueryParam: {kv.Key} = {kv.Value} ({kv.Value?.GetType().Name})");

        (_viewModel as IQueryAttributable)?.ApplyQueryAttributes(query);
    }

    /// <summary>
    /// Called when the page is navigated to. Logs the loaded recipe title for debugging.
    /// </summary>
    /// <param name="args">The navigation event arguments.</param>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine($"[RecipeDetailPage] OnNavigatedTo, Recipe={_viewModel.Recipe?.Title ?? "null"}");
    }
}
