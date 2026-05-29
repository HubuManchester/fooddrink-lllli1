using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    private readonly IDataService _dataService;
    private readonly ILocationService _locationService;

    [ObservableProperty]
    private ObservableCollection<Recipe> featuredRecipes = new();

    [ObservableProperty]
    private string locationGreeting = string.Empty;

    public HomePageViewModel(IDataService dataService, ILocationService locationService)
    {
        _dataService = dataService;
        _locationService = locationService;
        Title = "FoodLens";
    }

    [RelayCommand]
    private async Task LoadRecipesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var recipes = await _dataService.GetRecipesAsync();
            FeaturedRecipes = new ObservableCollection<Recipe>(recipes);

            _ = LoadLocationGreetingAsync();
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    private async Task LoadLocationGreetingAsync()
    {
        try
        {
            var location = await _locationService.GetCurrentLocationAsync();
            if (location is not null)
            {
                var address = await _locationService.GetAddressAsync(location.Latitude, location.Longitude);
                if (address is not null)
                {
                    LocationGreeting = $"Near {address}";
                }
            }
        }
        catch (Exception) { }
    }
}
