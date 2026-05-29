using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly ILocationService _locationService;

    [ObservableProperty]
    private string locationStatus = "Tap the button to get your location";

    [ObservableProperty]
    private string locationDisplay = string.Empty;

    [ObservableProperty]
    private bool hasLocation;

    public SettingsViewModel(ILocationService locationService)
    {
        _locationService = locationService;
        Title = "Settings";
    }

    [RelayCommand]
    private async Task GetLocationAsync()
    {
        IsBusy = true;
        LocationStatus = "Getting location...";
        try
        {
            var location = await _locationService.GetCurrentLocationAsync();
            if (location is not null)
            {
                LocationDisplay = $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";
                var address = await _locationService.GetAddressAsync(location.Latitude, location.Longitude);
                if (address is not null)
                {
                    LocationDisplay += $"\n{address}";
                }
                HasLocation = true;
                LocationStatus = "Location retrieved successfully";

                try
                {
                    HapticFeedback.Perform(HapticFeedbackType.Click);
                }
                catch (FeatureNotSupportedException) { }
                catch (Exception) { }
            }
            else
            {
                LocationStatus = "Location not available on this device";
            }
        }
        catch (FeatureNotSupportedException)
        {
            LocationStatus = "Geolocation is not supported on this device";
        }
        catch (PermissionException)
        {
            LocationStatus = "Location permission is required. Please grant access.";
        }
        catch (Exception)
        {
            LocationStatus = "Unable to get location. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
