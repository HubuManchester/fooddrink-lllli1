using System.Text.Json;

namespace FoodLens.Services;

/// <summary>
/// Provides GPS location, reverse geocoding via Nominatim API, and Leaflet.js map HTML generation.
/// </summary>
public class LocationService : ILocationService
{
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5),
        DefaultRequestHeaders =
        {
            { "User-Agent", "FoodLens/1.0 (foodlens.app@example.com)" }
        }
    };

    /// <summary>
    /// Requests location permissions and retrieves the current GPS location.
    /// </summary>
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            System.Diagnostics.Debug.WriteLine($"[FoodLens] Location permission: {status}");
            if (status == PermissionStatus.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(15));
                var location = await Geolocation.GetLocationAsync(request);
                System.Diagnostics.Debug.WriteLine($"[FoodLens] Geolocation result: {(location != null ? $"{location.Latitude},{location.Longitude}" : "null")}");
                if (location is not null)
                    return location;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FoodLens] Geolocation error: {ex.GetType().Name} - {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// Performs reverse geocoding to obtain a human-readable address from coordinates, using Nominatim first and falling back to the native Geocoding API.
    /// </summary>
    public async Task<string?> GetAddressAsync(double latitude, double longitude)
    {
        try
        {
            var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}&zoom=18&addressdetails=1";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("display_name", out var displayName))
                {
                    return displayName.GetString();
                }
            }
        }
        catch (Exception)
        {
        }

        try
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark is not null)
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(placemark.Thoroughfare)) parts.Add(placemark.Thoroughfare);
                if (!string.IsNullOrEmpty(placemark.Locality)) parts.Add(placemark.Locality);
                if (!string.IsNullOrEmpty(placemark.SubLocality)) parts.Add(placemark.SubLocality);
                if (!string.IsNullOrEmpty(placemark.AdminArea)) parts.Add(placemark.AdminArea);
                if (!string.IsNullOrEmpty(placemark.CountryName)) parts.Add(placemark.CountryName);
                if (!string.IsNullOrEmpty(placemark.FeatureName)) parts.Add(placemark.FeatureName);
                if (parts.Count > 0)
                    return string.Join(", ", parts);
            }
        }
        catch (Exception)
        {
        }

        return null;
    }

    /// <summary>
    /// Generates an HTML page with an embedded Leaflet.js map centered on the specified coordinates with a marker.
    /// </summary>
    public string GetMapHtml(double latitude, double longitude, string? label = null)
    {
        var markerLabel = label ?? "You are here";
        return $@"<!DOCTYPE html>
<html>
<head>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link rel=""stylesheet"" href=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.css"" />
    <script src=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.js""></script>
    <style>
        html, body {{ margin: 0; padding: 0; height: 100%; }}
        #map {{ height: 100%; width: 100%; }}
    </style>
</head>
<body>
    <div id=""map""></div>
    <script>
        var map = L.map('map').setView([{latitude}, {longitude}], 16);
        L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
            attribution: '&copy; OpenStreetMap contributors'
        }}).addTo(map);
        L.marker([{latitude}, {longitude}]).addTo(map)
            .bindPopup('{markerLabel}')
            .openPopup();
    </script>
</body>
</html>";
    }

    /// <summary>
    /// Opens the specified coordinates in the device's default maps application via Google Maps.
    /// </summary>
    public async Task OpenInMapsAsync(double latitude, double longitude, string? label = null)
    {
        var uri = new Uri($"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}");
        await Launcher.OpenAsync(uri);
    }
}
