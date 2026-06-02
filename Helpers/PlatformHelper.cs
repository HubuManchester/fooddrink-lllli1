namespace FoodLens.Helpers;

/// <summary>
/// Provides platform and device form-factor detection helpers.
/// </summary>
public static class PlatformHelper
{
    /// <summary>
    /// Gets a value indicating whether the current device is a tablet or desktop,
    /// which should use the wide/two-column layout.
    /// </summary>
    public static bool IsTabletOrDesktop =>
        DeviceInfo.Current.Platform == DevicePlatform.WinUI ||
        DeviceInfo.Current.Idiom == DeviceIdiom.Tablet ||
        DeviceInfo.Current.Idiom == DeviceIdiom.Desktop;
}
