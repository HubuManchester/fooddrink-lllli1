using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class SearchViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private string searchKeyword = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Recipe> searchResults = new();

    [ObservableProperty]
    private bool isEmpty;

    public SearchViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Search";
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchKeyword))
        {
            SearchResults.Clear();
            IsEmpty = false;
            return;
        }

        IsBusy = true;
        try
        {
            var results = await _dataService.SearchRecipesAsync(SearchKeyword);
            SearchResults = new ObservableCollection<Recipe>(results);
            IsEmpty = SearchResults.Count == 0;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task FilterByCategoryAsync(string category)
    {
        IsBusy = true;
        try
        {
            var results = await _dataService.SearchRecipesAsync(string.IsNullOrEmpty(category) ? "" : null, category);
            SearchResults = new ObservableCollection<Recipe>(results);
            IsEmpty = SearchResults.Count == 0;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
