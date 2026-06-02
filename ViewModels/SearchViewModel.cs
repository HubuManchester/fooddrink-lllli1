using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the search page, supporting keyword-based recipe search with debouncing
/// and category-based filtering via query attributes.
/// </summary>
public partial class SearchViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDataService _dataService;
    private CancellationTokenSource? _searchCts;

    /// <summary>
    /// Gets or sets the search keyword entered by the user. Changes trigger a debounced search.
    /// </summary>
    [ObservableProperty]
    private string searchKeyword = string.Empty;

    /// <summary>
    /// Gets or sets the collection of recipes matching the current search or category filter.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Recipe> searchResults = new();

    /// <summary>
    /// Gets or sets a value indicating whether the search returned no results.
    /// </summary>
    [ObservableProperty]
    private bool isEmpty;

    /// <summary>
    /// Gets or sets the currently selected category for filtering recipes.
    /// </summary>
    [ObservableProperty]
    private string selectedCategory = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for searching and filtering recipes.</param>
    public SearchViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Search";
    }

    /// <summary>
    /// Applies query attributes from navigation, supporting category-based filtering via the "category" parameter.
    /// </summary>
    /// <param name="query">The dictionary of navigation query parameters.</param>
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

    /// <summary>
    /// Called when <see cref="SearchKeyword"/> changes; cancels any pending search and starts a new debounced search.
    /// </summary>
    partial void OnSearchKeywordChanged(string value)
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        _ = DebouncedSearchAsync(_searchCts.Token);
    }

    /// <summary>
    /// Delays search execution by 300ms to debounce rapid keystrokes, then triggers the search.
    /// </summary>
    /// <param name="token">A cancellation token to cancel the debounced delay.</param>
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

    /// <summary>
    /// Executes a keyword search against the data service and updates search results.
    /// </summary>
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

    /// <summary>
    /// Filters recipes by the specified category and updates search results.
    /// </summary>
    /// <param name="category">The category name to filter by, or <c>null</c> for no filter.</param>
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
