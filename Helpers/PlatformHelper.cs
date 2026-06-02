namespace FoodLens.Helpers;

public static class PlatformHelper
{
    private static DisplayOrientation _lastOrientation = DisplayOrientation.Unknown;

    public static bool IsTabletOrDesktop =>
        DeviceInfo.Current.Platform == DevicePlatform.WinUI ||
        DeviceInfo.Current.Idiom == DeviceIdiom.Tablet ||
        DeviceInfo.Current.Idiom == DeviceIdiom.Desktop;

    public static bool IsWideLayout => DetermineWideLayout();

    public static event EventHandler<LayoutModeChangedEventArgs>? LayoutChanged;

    public static void StartMonitoring()
    {
        _lastOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
        DeviceDisplay.MainDisplayInfoChanged += OnDisplayInfoChanged;
    }

    public static void StopMonitoring()
    {
        DeviceDisplay.MainDisplayInfoChanged -= OnDisplayInfoChanged;
    }

    private static bool DetermineWideLayout()
    {
        var platform = DeviceInfo.Current.Platform;
        var idiom = DeviceInfo.Current.Idiom;
        var orientation = DeviceDisplay.MainDisplayInfo.Orientation;

        if (platform == DevicePlatform.WinUI || idiom == DeviceIdiom.Desktop)
            return true;

        if (idiom == DeviceIdiom.Tablet || idiom == DeviceIdiom.Desktop)
            return orientation == DisplayOrientation.Landscape;

        return false;
    }

    private static void OnDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
    {
        var newOrientation = e.DisplayInfo.Orientation;
        if (newOrientation == _lastOrientation)
            return;

        var wasWide = DetermineWideLayoutFromOrientation(_lastOrientation);
        _lastOrientation = newOrientation;
        var isWide = DetermineWideLayoutFromOrientation(newOrientation);

        if (wasWide != isWide)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LayoutChanged?.Invoke(null, new LayoutModeChangedEventArgs(isWide));
            });
        }
    }

    private static bool DetermineWideLayoutFromOrientation(DisplayOrientation orientation)
    {
        var platform = DeviceInfo.Current.Platform;
        var idiom = DeviceInfo.Current.Idiom;

        if (platform == DevicePlatform.WinUI || idiom == DeviceIdiom.Desktop)
            return true;

        if (idiom == DeviceIdiom.Tablet || idiom == DeviceIdiom.Desktop)
            return orientation == DisplayOrientation.Landscape;

        return false;
    }
}

public class LayoutModeChangedEventArgs : EventArgs
{
    public bool IsWideLayout { get; }

    public LayoutModeChangedEventArgs(bool isWideLayout)
    {
        IsWideLayout = isWideLayout;
    }
}
