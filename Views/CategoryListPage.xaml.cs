using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class CategoryListPage : ContentPage
{
    public CategoryListPage(CategoryListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
