namespace FoodLens.Views;

/// <summary>
/// Represents the splash/loading screen displayed when the application starts.
/// Plays a sequential entry animation before navigating to the main shell.
/// </summary>
public partial class SplashPage : ContentPage
{
    /// <summary>
    /// Static event fired when the splash animation completes. Set by App.xaml.cs.
    /// </summary>
    public static Action? DismissRequested;

    public SplashPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        try
        {
            await AnimateEntryAsync();
            await Task.Delay(600);
            await this.FadeTo(0, 400, Easing.CubicIn);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FoodLens] Splash animation error: {ex.Message}");
        }

        DismissRequested?.Invoke();
    }

    /// <summary>
    /// Plays the sequential fade-in and slide-up animation for each UI element.
    /// </summary>
    private async Task AnimateEntryAsync()
    {
        LogoIcon.Scale = 0.5;
        var logoFade = LogoIcon.FadeTo(1, 600, Easing.CubicOut);
        var logoScale = LogoIcon.ScaleTo(1, 600, Easing.SpringOut);
        await Task.WhenAll(logoFade, logoScale);
        await Task.Delay(150);

        var titleTask = AnimateElement(TitleLabel, 400);
        await Task.Delay(100);

        var subtitleTask = AnimateElement(SubtitleLabel, 400);
        await Task.Delay(150);

        DividerLine.WidthRequest = 60;
        var dividerFade = DividerLine.FadeTo(1, 400, Easing.CubicOut);
        await dividerFade;
        await Task.Delay(100);

        var loadingTask = AnimateElement(LoadingIndicator, 300);
        var labelTask = AnimateElement(LoadingLabel, 300);
        await Task.WhenAll(titleTask, subtitleTask, loadingTask, labelTask);

        await LogoIcon.ScaleTo(1.08, 400, Easing.SinInOut);
        await LogoIcon.ScaleTo(1.0, 300, Easing.SinInOut);
    }

    /// <summary>
    /// Animates a single element with a fade-in and slide-up effect.
    /// </summary>
    private async Task AnimateElement(VisualElement element, uint duration)
    {
        var fadeTask = element.FadeTo(1, duration, Easing.CubicOut);
        var slideTask = element.TranslateTo(0, 0, duration, Easing.CubicOut);
        await Task.WhenAll(fadeTask, slideTask);
    }
}
