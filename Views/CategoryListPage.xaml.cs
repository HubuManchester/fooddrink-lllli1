using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class CategoryListPage : ContentPage
{
    private readonly CategoryListViewModel _viewModel;

    public CategoryListPage(CategoryListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _viewModel.LoadCategoriesCommand.ExecuteAsync(null);
    }
}
