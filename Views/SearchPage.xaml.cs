using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        ApplyLayout(PlatformHelper.IsWideLayout);
        PlatformHelper.LayoutChanged += OnLayoutChanged;
    }

    private void OnLayoutChanged(object? sender, LayoutModeChangedEventArgs e)
    {
        ApplyLayout(e.IsWideLayout);
    }

    private void ApplyLayout(bool wide)
    {
        SearchResultsCollectionView.ItemsLayout = wide
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
    }
}
