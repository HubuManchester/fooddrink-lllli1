namespace FoodLens.Services;

public class LocationService : ILocationService
{
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);
            if (location is null)
            {
                location = new Location(39.9042, 116.4074);
            }
            return location;
        }
        catch (FeatureNotSupportedException)
        {
            return new Location(39.9042, 116.4074);
        }
        catch (PermissionException)
        {
            return null;
        }
        catch (Exception)
        {
            return new Location(39.9042, 116.4074);
        }
    }

    public async Task<string?> GetAddressAsync(double latitude, double longitude)
    {
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
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
