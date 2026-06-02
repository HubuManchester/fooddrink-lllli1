using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the home page, displaying featured recipes, a location-based greeting,
/// user authentication status, and shake-to-discover random recipe functionality.
/// </summary>
public partial class HomePageViewModel : BaseViewModel
{
    private readonly IDataService _dataService;
    private readonly ILocationService _locationService;
    private readonly IAccelerometerService _accelerometerService;
    private readonly Random _random = new();

    /// <summary>
    /// Gets or sets the collection of featured recipes displayed on the home page.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Recipe> featuredRecipes = new();

    /// <summary>
    /// Gets or sets the location-based greeting text (e.g., "Near New York").
    /// </summary>
    [ObservableProperty]
    private string locationGreeting = string.Empty;

    /// <summary>
    /// Gets or sets the user greeting text showing display name and email when logged in.
    /// </summary>
    [ObservableProperty]
    private string userGreeting = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user is currently authenticated.
    /// </summary>
    [ObservableProperty]
    private bool isLoggedIn;

    /// <summary>
    /// Gets or sets the hint text encouraging the user to shake their device for a random recipe.
    /// Empty when accelerometer is not supported.
    /// </summary>
    [ObservableProperty]
    private string shakeHint = "Shake your device to discover a random recipe!";

    /// <summary>
    /// Initializes a new instance of the <see cref="HomePageViewModel"/> class with injected services.
    /// </summary>
    /// <param name="dataService">The data service for accessing recipe and user data.</param>
    /// <param name="locationService">The location service for retrieving the user's current location.</param>
    /// <param name="accelerometerService">The accelerometer service for detecting device shakes.</param>
    public HomePageViewModel(IDataService dataService, ILocationService locationService, IAccelerometerService accelerometerService)
    {
        _dataService = dataService;
        _locationService = locationService;
        _accelerometerService = accelerometerService;
        Title = "FoodLens";

        if (_accelerometerService.IsSupported)
        {
            _accelerometerService.ShakeDetected += OnShakeDetected;
            _accelerometerService.StartMonitoring();
        }
        else
        {
            ShakeHint = string.Empty;
        }
    }

    /// <summary>
    /// Handles the accelerometer shake event by selecting a random featured recipe
    /// and prompting the user to view it.
    /// </summary>
    private async void OnShakeDetected(object? sender, EventArgs e)
    {
        if (FeaturedRecipes.Count == 0) return;

        var index = _random.Next(FeaturedRecipes.Count);
        var recipe = FeaturedRecipes[index];

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var view = await Shell.Current.DisplayAlert(
                "Random Recipe!",
                $"{recipe.Title}\n{recipe.Description}\n\nCook time: {recipe.CookTimeMinutes} min | {recipe.Difficulty}",
                "View Recipe", "OK");
            if (view)
            {
                await Shell.Current.GoToAsync($"recipedetail?id={recipe.Id}");
            }
        });
    }

    /// <summary>
    /// Simulates a device shake for testing purposes, triggering the same behavior as a real shake.
    /// </summary>
    [RelayCommand]
    private void SimulateShake()
    {
        OnShakeDetected(this, EventArgs.Empty);
    }

    /// <summary>
    /// Loads the featured recipes from the data service and initiates user greeting lookup.
    /// </summary>
    [RelayCommand]
    private async Task LoadRecipesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var recipes = await _dataService.GetRecipesAsync();
            FeaturedRecipes = new ObservableCollection<Recipe>(recipes);

            _ = Task.Run(async () =>
            {
                try { await LoadUserGreetingAsync(); } catch { }
            });
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Loads the location-based greeting by retrieving the user's current address.
    /// </summary>
    private async Task LoadLocationGreetingAsync()
    {
        try
        {
            var location = await _locationService.GetCurrentLocationAsync();
            if (location is not null)
            {
                var address = await _locationService.GetAddressAsync(location.Latitude, location.Longitude);
                if (!string.IsNullOrWhiteSpace(address))
                {
                    LocationGreeting = $"Near {address}";
                }
            }
        }
        catch (Exception) { }
    }

    /// <summary>
    /// Loads the authenticated user's display name and email for the greeting area.
    /// </summary>
    private async Task LoadUserGreetingAsync()
    {
        try
        {
            var user = await _dataService.GetCurrentUserAsync();
            if (user is not null)
            {
                IsLoggedIn = true;
                UserGreeting = $"{user.DisplayName}\n{user.Email}";
            }
            else
            {
                IsLoggedIn = false;
                UserGreeting = string.Empty;
            }
        }
        catch (Exception) { }
    }

    /// <summary>
    /// Navigates to the recipe creation/edit page for a new recipe.
    /// </summary>
    [RelayCommand]
    private static async Task CreateRecipeAsync()
    {
        await Shell.Current.GoToAsync("recipeedit");
    }

    /// <summary>
    /// Toggles the favorite status of a recipe and updates the local collection.
    /// </summary>
    [RelayCommand]
    private async Task ToggleHomeFavoriteAsync(int recipeId)
    {
        await _dataService.ToggleFavoriteAsync(recipeId);

        var index = FeaturedRecipes.ToList().FindIndex(r => r.Id == recipeId);
        if (index < 0) return;

        var updated = await _dataService.GetRecipeByIdAsync(recipeId);
        if (updated is not null)
        {
            FeaturedRecipes[index] = updated;
        }
    }
}
