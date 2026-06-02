using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// Base ViewModel providing common services, navigation helpers, and favorite toggle commands
/// for all page-specific ViewModels in the FoodLens application.
/// </summary>
public partial class BaseViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel is currently performing a busy operation.
    /// </summary>
    [ObservableProperty]
    private bool isBusy;

    /// <summary>
    /// Gets or sets the page title displayed in the navigation bar.
    /// </summary>
    [ObservableProperty]
    private string title = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the current view is in a refreshing state (pull-to-refresh).
    /// </summary>
    [ObservableProperty]
    private bool isRefreshing;

    private static IServiceProvider? _serviceProvider;

    /// <summary>
    /// Sets the shared service provider instance used for dependency resolution across all ViewModels.
    /// </summary>
    /// <param name="sp">The application's <see cref="IServiceProvider"/>.</param>
    internal static void SetServiceProvider(IServiceProvider sp) => _serviceProvider = sp;

    /// <summary>
    /// Gets the current shared service provider instance.
    /// </summary>
    /// <returns>The registered <see cref="IServiceProvider"/>, or <c>null</c> if not set.</returns>
    internal static IServiceProvider? GetServiceProvider() => _serviceProvider;

    /// <summary>
    /// Navigates to the recipe detail page for the specified recipe.
    /// </summary>
    /// <param name="recipeId">The unique identifier of the recipe to view.</param>
    [RelayCommand]
    private static async Task NavigateToRecipeAsync(int recipeId)
    {
        await Shell.Current.GoToAsync($"recipedetail?id={recipeId}");
    }

    /// <summary>
    /// Toggles the favorite status of the specified recipe.
    /// </summary>
    /// <param name="recipeId">The unique identifier of the recipe to favorite or unfavorite.</param>
    [RelayCommand]
    private static async Task ToggleFavoriteAsync(int recipeId)
    {
        if (_serviceProvider is null) return;
        var dataService = _serviceProvider.GetRequiredService<IDataService>();
        await dataService.ToggleFavoriteAsync(recipeId);
    }
}
