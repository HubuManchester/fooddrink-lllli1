using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Recipe> favoriteRecipes = new();

    [ObservableProperty]
    private bool isRefreshing;

    public FavoritesViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Favorites";
    }

    [RelayCommand]
    private async Task LoadFavoritesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var favorites = await _dataService.GetFavoriteRecipesAsync();
            FavoriteRecipes = new ObservableCollection<Recipe>(favorites);
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
