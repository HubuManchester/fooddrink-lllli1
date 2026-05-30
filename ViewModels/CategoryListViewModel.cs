using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class CategoryListViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    public CategoryListViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Categories";
    }

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

    [RelayCommand]
    private static async Task NavigateToCategoryAsync(string categoryName)
    {
        await Shell.Current.GoToAsync($"///search?category={Uri.EscapeDataString(categoryName)}");
    }
}
