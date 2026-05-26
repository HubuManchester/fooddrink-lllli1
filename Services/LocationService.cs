namespace FoodLens.Services;

public class LocationService : ILocationService
{
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            return await Geolocation.GetLocationAsync(request);
        }
        catch (FeatureNotSupportedException)
        {
            return null;
        }
        catch (PermissionException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
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
                return $"{placemark.Thoroughfare}, {placemark.Locality}";
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
