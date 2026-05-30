using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class SearchViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDataService _dataService;
    private CancellationTokenSource? _searchCts;

    [ObservableProperty]
    private string searchKeyword = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Recipe> searchResults = new();

    [ObservableProperty]
    private bool isEmpty;

    [ObservableProperty]
    private string selectedCategory = string.Empty;

    public SearchViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Search";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("category", out var category))
        {
            var cat = category?.ToString() ?? string.Empty;
            SelectedCategory = cat;
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await FilterByCategoryAsync(cat);
            });
        }
    }

    partial void OnSearchKeywordChanged(string value)
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        _ = DebouncedSearchAsync(_searchCts.Token);
    }

    private async Task DebouncedSearchAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(300, token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await SearchAsync();
        });
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
    private async Task FilterByCategoryAsync(string? category)
    {
        IsBusy = true;
        try
        {
            var cat = string.IsNullOrEmpty(category) ? null : category;
            var results = await _dataService.SearchRecipesAsync(string.Empty, cat);
            SearchResults = new ObservableCollection<Recipe>(results);
            IsEmpty = SearchResults.Count == 0;
            SelectedCategory = category ?? string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
