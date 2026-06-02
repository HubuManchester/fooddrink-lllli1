using FoodLens.Helpers;
using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the home page displaying featured recipes with platform-specific layout adjustments.
/// </summary>
public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel _viewModel;
    private static bool _splashPlayed;

    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        if (PlatformHelper.IsTabletOrDesktop)
        {
            FeaturedRecipesCollectionView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = 4,
                VerticalItemSpacing = 4
            };
        }
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!_splashPlayed)
        {
            _splashPlayed = true;
            await PlaySplashAsync();
        }

        try { await _viewModel.LoadRecipesCommand.ExecuteAsync(null); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[FoodLens] HomePage.OnNavigatedTo: {ex.Message}"); }
    }

    private async Task PlaySplashAsync()
    {
        SplashOverlay.IsVisible = true;
        SplashOverlay.Opacity = 1;

        SplashLogo.Scale = 0.5;
        await Task.WhenAll(
            SplashLogo.FadeTo(1, 500, Easing.CubicOut),
            SplashLogo.ScaleTo(1, 500, Easing.SpringOut));
        await Task.Delay(100);

        await Task.WhenAll(
            SplashTitle.FadeTo(1, 350, Easing.CubicOut),
            SplashTitle.TranslateTo(0, 0, 350, Easing.CubicOut));
        await Task.Delay(80);

        await Task.WhenAll(
            SplashSubtitle.FadeTo(1, 350, Easing.CubicOut),
            SplashSubtitle.TranslateTo(0, 0, 350, Easing.CubicOut));
        await Task.Delay(80);

        await SplashSpinner.FadeTo(1, 250, Easing.CubicOut);
        await Task.Delay(400);

        await SplashLogo.ScaleTo(1.06, 300, Easing.SinInOut);
        await SplashLogo.ScaleTo(1.0, 250, Easing.SinInOut);
        await Task.Delay(200);

        await SplashOverlay.FadeTo(0, 350, Easing.CubicIn);
        SplashOverlay.IsVisible = false;
    }
}
