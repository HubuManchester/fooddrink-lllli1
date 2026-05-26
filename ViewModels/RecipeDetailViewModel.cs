using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class RecipeDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDataService _dataService;
    private readonly ISpeechService _speechService;

    [ObservableProperty]
    private Recipe recipe = new();

    public RecipeDetailViewModel(IDataService dataService, ISpeechService speechService)
    {
        _dataService = dataService;
        _speechService = speechService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && id is int recipeId)
        {
            _ = LoadRecipeAsync(recipeId);
        }
    }

    [RelayCommand]
    private async Task LoadRecipeAsync(int recipeId)
    {
        IsBusy = true;
        try
        {
            var recipe = await _dataService.GetRecipeByIdAsync(recipeId);
            if (recipe is not null) Recipe = recipe;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync()
    {
        await _dataService.ToggleFavoriteAsync(Recipe.Id);
        Recipe.IsFavorite = !Recipe.IsFavorite;
        OnPropertyChanged(nameof(Recipe));
    }

    [RelayCommand]
    private async Task ReadStepsAsync()
    {
        foreach (var step in Recipe.Instructions)
        {
            await _speechService.SpeakAsync(step);
        }
    }
}
