using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the search page where users can search for recipes with platform-specific layout adjustments.
/// </summary>
public partial class SearchPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPage"/> class with the specified view model.
    /// Configures platform-specific layout for the search results collection.
    /// </summary>
    /// <param name="viewModel">The search view model that handles search operations.</param>
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        if (PlatformHelper.IsTabletOrDesktop)
        {
            SearchResultsCollectionView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = 4,
                VerticalItemSpacing = 4
            };
        }
    }
}
