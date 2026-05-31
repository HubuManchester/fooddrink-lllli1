using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

public partial class RecipeDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDataService _dataService;
    private readonly ISpeechService _speechService;
    private readonly ICameraService _cameraService;
    private readonly IVisionService _visionService;

    [ObservableProperty]
    private Recipe recipe = new();

    [ObservableProperty]
    private bool isReading;

    [ObservableProperty]
    private string readButtonText = "Read Steps";

    [ObservableProperty]
    private string? capturedPhotoPath;

    [ObservableProperty]
    private List<string> recognizedIngredients = new();

    [ObservableProperty]
    private bool hasRecognizedIngredients;

    public RecipeDetailViewModel(IDataService dataService, ISpeechService speechService, ICameraService cameraService, IVisionService visionService)
    {
        _dataService = dataService;
        _speechService = speechService;
        _cameraService = cameraService;
        _visionService = visionService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && int.TryParse(id?.ToString(), out var recipeId))
        {
            _ = LoadRecipeAsync(recipeId);
        }
    }

    [RelayCommand]
    private async Task LoadRecipeAsync(int recipeId)
    {
        IsBusy = true;
        try
        {
            var recipe = await _dataService.GetRecipeByIdAsync(recipeId);
            if (recipe is not null) Recipe = recipe;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ToggleDetailFavoriteAsync()
    {
        await _dataService.ToggleFavoriteAsync(Recipe.Id);
        Recipe.IsFavorite = !Recipe.IsFavorite;
        OnPropertyChanged(nameof(Recipe));

        try
        {
            HapticFeedback.Perform(HapticFeedbackType.LongPress);
            if (Recipe.IsFavorite)
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(100));
            }
        }
        catch (FeatureNotSupportedException) { }
        catch (Exception) { }
    }

    private CancellationTokenSource? _readCts;

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ReadStepsAsync()
    {
        if (IsReading)
        {
            _readCts?.Cancel();
            return;
        }

        IsReading = true;
        ReadButtonText = "Stop Reading";
        _readCts = new CancellationTokenSource();

        try
        {
            foreach (var step in Recipe.Instructions)
            {
                _readCts.Token.ThrowIfCancellationRequested();
                await _speechService.SpeakAsync(step);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            IsReading = false;
            ReadButtonText = "Read Steps";
            _readCts?.Dispose();
            _readCts = null;
        }
    }

    [RelayCommand]
    private async Task CaptureAndRecognizeAsync()
    {
        IsBusy = true;
        try
        {
            var photoPath = await _cameraService.CapturePhotoAsync();
            if (photoPath is null) return;

            CapturedPhotoPath = photoPath;

            var bytes = await _cameraService.GetPhotoBytesAsync(photoPath);
            if (bytes is null) return;

            var ingredients = await _visionService.RecognizeIngredientsAsync(bytes);
            RecognizedIngredients = ingredients;
            HasRecognizedIngredients = ingredients.Count > 0;
        }
        catch (FeatureNotSupportedException)
        {
            await Shell.Current.DisplayAlert("Not Supported", "Camera is not available on this device.", "OK");
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlert("Permission Required", "Please grant camera permission to use this feature.", "OK");
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Error", "An error occurred while capturing photo.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
