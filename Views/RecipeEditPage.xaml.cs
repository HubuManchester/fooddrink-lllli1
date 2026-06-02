using System.Diagnostics;
using FoodLens.ViewModels;

namespace FoodLens.Views;

/// <summary>
/// Represents the recipe create/edit form page for adding or modifying recipes.
/// </summary>
public partial class RecipeEditPage : ContentPage, IQueryAttributable
{
    private readonly RecipeEditViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeEditPage"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The recipe edit view model that handles recipe creation and modification.</param>
    public RecipeEditPage(RecipeEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    /// <summary>
    /// Applies query attributes received during navigation, forwarding them to the view model.
    /// </summary>
    /// <param name="query">A dictionary of navigation query parameters.</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        foreach (var kv in query)
            Debug.WriteLine($"[RecipeEditPage] QueryParam: {kv.Key} = {kv.Value}");

        (_viewModel as IQueryAttributable)?.ApplyQueryAttributes(query);
    }
}
