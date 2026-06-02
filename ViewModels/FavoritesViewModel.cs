using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the favorites page, displaying a list of recipes the user has marked as favorites.
/// </summary>
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Gets or sets the collection of recipes the user has favorited.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Recipe> favoriteRecipes = new();

    /// <summary>
    /// Gets or sets a value indicating whether the favorites list is empty.
    /// </summary>
    [ObservableProperty]
    private bool isEmpty = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoritesViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for retrieving favorite recipes.</param>
    public FavoritesViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Favorites";
    }

    /// <summary>
    /// Loads the user's favorite recipes from the data service.
    /// </summary>
    [RelayCommand]
    private async Task LoadFavoritesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var favorites = await _dataService.GetFavoriteRecipesAsync();
            FavoriteRecipes = new ObservableCollection<Recipe>(favorites);
            IsEmpty = FavoriteRecipes.Count == 0;
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
