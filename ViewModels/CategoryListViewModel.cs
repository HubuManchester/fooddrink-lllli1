using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the category browsing page, displaying available recipe categories
/// and providing navigation to category-filtered search results.
/// </summary>
public partial class CategoryListViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Gets or sets the collection of recipe categories displayed to the user.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryListViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for retrieving recipe categories.</param>
    public CategoryListViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Categories";
    }

    /// <summary>
    /// Loads all available recipe categories from the data service.
    /// </summary>
    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var cats = await _dataService.GetCategoriesAsync();
            Categories = new ObservableCollection<Category>(cats);
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Navigates to the search page filtered by the specified category name.
    /// </summary>
    /// <param name="categoryName">The name of the category to filter by.</param>
    [RelayCommand]
    private static async Task NavigateToCategoryAsync(string categoryName)
    {
        await Shell.Current.GoToAsync($"///search?category={Uri.EscapeDataString(categoryName)}");
    }
}
