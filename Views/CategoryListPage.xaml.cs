using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the category browsing page where users can explore recipe categories.
/// </summary>
public partial class CategoryListPage : ContentPage
{
    private readonly CategoryListViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryListPage"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The category list view model that provides category data.</param>
    public CategoryListPage(CategoryListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    /// <summary>
    /// Called when the page is navigated to. Loads categories from the view model.
    /// </summary>
    /// <param name="args">The navigation event arguments.</param>
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        try { await _viewModel.LoadCategoriesCommand.ExecuteAsync(null); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[FoodLens] CategoryListPage.OnNavigatedTo: {ex.Message}"); }
    }
}
