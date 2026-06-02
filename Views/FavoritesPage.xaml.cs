using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesViewModel _viewModel;

    public FavoritesPage(FavoritesViewModel viewModel)
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
        FavoritesCollectionView.ItemsLayout = wide
            ? new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = 4,
                VerticalItemSpacing = 4
            }
            : new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 4
            };
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

        try { _viewModel.LoadFavoritesCommand.ExecuteAsync(null); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[FoodLens] FavoritesPage.OnNavigatedTo: {ex.Message}"); }
    }
}
