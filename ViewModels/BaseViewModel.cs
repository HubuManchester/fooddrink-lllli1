using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isRefreshing;

    private static IServiceProvider? _serviceProvider;

    internal static void SetServiceProvider(IServiceProvider sp) => _serviceProvider = sp;

    [RelayCommand]
    private static async Task NavigateToRecipeAsync(int recipeId)
    {
        await Shell.Current.GoToAsync($"recipedetail?id={recipeId}");
    }

    [RelayCommand]
    private static async Task ToggleFavoriteAsync(int recipeId)
    {
        if (_serviceProvider is null) return;
        var dataService = _serviceProvider.GetRequiredService<IDataService>();
        await dataService.ToggleFavoriteAsync(recipeId);
    }
}
