using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class MealPlanViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private ObservableCollection<MealPlan> mealPlans = new();

    [ObservableProperty]
    private bool isEmpty;

    public MealPlanViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Meal Plan";
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadMealPlansAsync();
    }

    [RelayCommand]
    private async Task LoadMealPlansAsync()
    {
        IsBusy = true;
        try
        {
            var plans = await _dataService.GetMealPlansAsync(SelectedDate);
            MealPlans = new ObservableCollection<MealPlan>(plans);
            IsEmpty = MealPlans.Count == 0;
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
