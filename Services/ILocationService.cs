namespace FoodLens.Services;

/// <summary>
/// Provides GPS location, reverse geocoding, and map display functionality.
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Gets the device's current geographic location.
    /// </summary>
    /// <returns>The current location, or null if unavailable.</returns>
    Task<Location?> GetCurrentLocationAsync();

    /// <summary>
    /// Reverse-geocodes coordinates into a human-readable address.
    /// </summary>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <returns>The address string, or null if geocoding failed.</returns>
    Task<string?> GetAddressAsync(double latitude, double longitude);

    /// <summary>
    /// Generates an HTML page with an embedded Leaflet.js map showing a pin at the specified location.
    /// </summary>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="label">Optional label for the map pin.</param>
    /// <returns>HTML string for display in a WebView.</returns>
    string GetMapHtml(double latitude, double longitude, string? label = null);

    /// <summary>
    /// Opens the specified location in the system's maps application.
    /// </summary>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="label">Optional label for the map pin.</param>
    Task OpenInMapsAsync(double latitude, double longitude, string? label = null);
}
