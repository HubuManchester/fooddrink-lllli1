using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the favorites page that lists the user's saved recipes with platform-specific layout adjustments.
/// </summary>
public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoritesPage"/> class with the specified view model.
    /// Configures platform-specific layout for the favorites collection.
    /// </summary>
    /// <param name="viewModel">The favorites view model that provides saved recipe data.</param>
    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        if (PlatformHelper.IsTabletOrDesktop)
        {
            FavoritesCollectionView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = 4,
                VerticalItemSpacing = 4
            };
        }
    }

    /// <summary>
    /// Called when the page is navigated to. Loads the user's favorite recipes from the view model.
    /// </summary>
    /// <param name="args">The navigation event arguments.</param>
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        try { await _viewModel.LoadFavoritesCommand.ExecuteAsync(null); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[FoodLens] FavoritesPage.OnNavigatedTo: {ex.Message}"); }
    }
}
