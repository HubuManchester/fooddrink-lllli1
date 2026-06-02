using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the settings page, providing account management (login/logout),
/// appearance customization (font size), and location services with map display.
/// </summary>
public partial class SettingsViewModel : BaseViewModel
{
    private readonly ILocationService _locationService;
    private readonly IDataService _dataService;

    private double _lastLatitude;
    private double _lastLongitude;

    /// <summary>
    /// Gets or sets the status message for the location retrieval process.
    /// </summary>
    [ObservableProperty]
    private string locationStatus = "Tap the button to get your location";

    /// <summary>
    /// Gets or sets the human-readable location address or coordinates display text.
    /// </summary>
    [ObservableProperty]
    private string locationDisplay = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user's location has been successfully retrieved.
    /// </summary>
    [ObservableProperty]
    private bool hasLocation;

    /// <summary>
    /// Gets or sets the HTML web view source for displaying the embedded map.
    /// </summary>
    [ObservableProperty]
    private HtmlWebViewSource mapSource = new();

    /// <summary>
    /// Gets or sets a value indicating whether a map is available to display.
    /// </summary>
    [ObservableProperty]
    private bool hasMap;

    /// <summary>
    /// Gets or sets a value indicating whether the user is currently authenticated.
    /// </summary>
    [ObservableProperty]
    private bool isLoggedIn;

    /// <summary>
    /// Gets or sets the display name of the authenticated user.
    /// </summary>
    [ObservableProperty]
    private string userDisplayName = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the authenticated user.
    /// </summary>
    [ObservableProperty]
    private string userEmail = string.Empty;

    /// <summary>
    /// Gets or sets the index of the selected font size preset (0=Small, 1=Medium, 2=Large).
    /// Persisted in application preferences.
    /// </summary>
    [ObservableProperty]
    private int fontSizeSetting = Preferences.Default.Get("font_size_setting", 1);

    /// <summary>
    /// Gets the list of available font size option labels.
    /// </summary>
    public List<string> FontSizeOptions { get; } = new() { "Small", "Medium", "Large" };

    private static readonly Dictionary<string, Dictionary<string, double>> FontSizePresets = new()
    {
        ["Small"] = new()
        {
            ["FontSizeSmall"] = 10, ["FontSizeDefault"] = 12, ["FontSizeMedium"] = 14,
            ["FontSizeLarge"] = 18, ["FontSizeTitle"] = 20, ["FontSizeHeadline"] = 26, ["FontSizeDisplay"] = 32
        },
        ["Medium"] = new()
        {
            ["FontSizeSmall"] = 12, ["FontSizeDefault"] = 14, ["FontSizeMedium"] = 16,
            ["FontSizeLarge"] = 20, ["FontSizeTitle"] = 24, ["FontSizeHeadline"] = 32, ["FontSizeDisplay"] = 40
        },
        ["Large"] = new()
        {
            ["FontSizeSmall"] = 14, ["FontSizeDefault"] = 17, ["FontSizeMedium"] = 19,
            ["FontSizeLarge"] = 24, ["FontSizeTitle"] = 28, ["FontSizeHeadline"] = 38, ["FontSizeDisplay"] = 48
        }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class with dependency injection.
    /// Applies the persisted font size setting on construction.
    /// </summary>
    /// <param name="locationService">The location service for geolocation and map functionality.</param>
    /// <param name="dataService">The data service for user authentication and data access.</param>
    public SettingsViewModel(ILocationService locationService, IDataService dataService)
    {
        _locationService = locationService;
        _dataService = dataService;
        Title = "Settings";

        ApplyFontSize(FontSizeOptions[FontSizeSetting]);
    }

    /// <summary>
    /// Loads the current user's authentication state and profile information.
    /// </summary>
    public async Task LoadUserAsync()
    {
        var user = await _dataService.GetCurrentUserAsync();
        if (user is not null)
        {
            IsLoggedIn = true;
            UserDisplayName = user.DisplayName;
            UserEmail = user.Email;
        }
        else
        {
            IsLoggedIn = false;
            UserDisplayName = string.Empty;
            UserEmail = string.Empty;
        }
    }

    /// <summary>
    /// Navigates to the login page for user authentication.
    /// </summary>
    [RelayCommand]
    private async Task LoginAsync()
    {
        await Shell.Current.GoToAsync("login");
    }

    /// <summary>
    /// Logs out the current user and clears the displayed user information.
    /// </summary>
    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _dataService.LogoutAsync();
        IsLoggedIn = false;
        UserDisplayName = string.Empty;
        UserEmail = string.Empty;
    }

    /// <summary>
    /// Retrieves the user's current geographic location, resolves the address, and displays an embedded map.
    /// </summary>
    [RelayCommand]
    private async Task GetLocationAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        LocationStatus = "Getting location...";
        try
        {
            var location = await _locationService.GetCurrentLocationAsync()
                .WaitAsync(TimeSpan.FromSeconds(15));
            if (location is not null)
            {
                _lastLatitude = location.Latitude;
                _lastLongitude = location.Longitude;

                var address = await Task.Run(() => _locationService.GetAddressAsync(location.Latitude, location.Longitude))
                    .WaitAsync(TimeSpan.FromSeconds(8));
                if (!string.IsNullOrWhiteSpace(address))
                {
                    LocationDisplay = address;
                }
                else
                {
                    LocationDisplay = $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";
                }

                MapSource = new HtmlWebViewSource { Html = _locationService.GetMapHtml(location.Latitude, location.Longitude, address) };
                HasMap = true;
                HasLocation = true;
                LocationStatus = "Location retrieved successfully";

                try
                {
                    HapticFeedback.Perform(HapticFeedbackType.Click);
                    System.Diagnostics.Debug.WriteLine("[FoodLens] HapticFeedback.Click triggered (location retrieved)");
                }
                catch (FeatureNotSupportedException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[FoodLens] HapticFeedback not supported: {ex.Message}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[FoodLens] HapticFeedback error: {ex.Message}");
                }
            }
            else
            {
                LocationStatus = "Location unavailable. Please enable location services in Windows Settings > Privacy & Security > Location.";
            }
        }
        catch (TimeoutException)
        {
            LocationStatus = "Location request timed out. Please try again.";
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

    /// <summary>
    /// Opens the retrieved location in the device's native maps application.
    /// </summary>
    [RelayCommand]
    private async Task OpenInMapsAsync()
    {
        if (HasLocation)
        {
            await _locationService.OpenInMapsAsync(_lastLatitude, _lastLongitude, LocationDisplay);
        }
    }

    /// <summary>
    /// Called when <see cref="FontSizeSetting"/> changes; persists the selection and applies the font size preset.
    /// </summary>
    partial void OnFontSizeSettingChanged(int value)
    {
        Preferences.Default.Set("font_size_setting", value);
        ApplyFontSize(FontSizeOptions[value]);
    }

    /// <summary>
    /// Applies the specified font size preset to the application's global resource dictionary.
    /// </summary>
    /// <param name="preset">The name of the font size preset ("Small", "Medium", or "Large").</param>
    private static void ApplyFontSize(string preset)
    {
        if (!FontSizePresets.TryGetValue(preset, out var sizes)) return;
        var resources = Application.Current!.Resources;
        foreach (var kvp in sizes)
        {
            resources[kvp.Key] = kvp.Value;
        }
    }
}
