using System.Diagnostics;
using FoodLens.ViewModels;

namespace FoodLens.Views;

public partial class RecipeDetailPage : ContentPage, IQueryAttributable
{
    private readonly RecipeDetailViewModel _viewModel;

    public RecipeDetailPage(RecipeDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        foreach (var kv in query)
            Debug.WriteLine($"[RecipeDetailPage] QueryParam: {kv.Key} = {kv.Value} ({kv.Value?.GetType().Name})");

        (_viewModel as IQueryAttributable)?.ApplyQueryAttributes(query);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine($"[RecipeDetailPage] OnNavigatedTo, Recipe={_viewModel.Recipe?.Title ?? "null"}");
    }
}
