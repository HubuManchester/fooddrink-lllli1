using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Recipe> featuredRecipes = new();

    [ObservableProperty]
    private bool isRefreshing;

    public HomePageViewModel(IDataService dataService)
    {
        _dataService = dataService;
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
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
