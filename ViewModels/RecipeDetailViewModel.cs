using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodLens.Models;
using FoodLens.Services;

namespace FoodLens.ViewModels;

/// <summary>
/// ViewModel for the recipe detail page, providing full recipe display with support for
/// favoriting, text-to-speech step reading, camera-based ingredient scanning via AI vision,
/// and recipe editing or deletion.
/// </summary>
public partial class RecipeDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDataService _dataService;
    private readonly ISpeechService _speechService;
    private readonly ICameraService _cameraService;
    private readonly IVisionService _visionService;

    /// <summary>
    /// Gets or sets the currently displayed recipe.
    /// </summary>
    [ObservableProperty]
    private Recipe recipe = new();

    /// <summary>
    /// Gets or sets a value indicating whether text-to-speech is currently reading recipe steps.
    /// </summary>
    [ObservableProperty]
    private bool isReading;

    /// <summary>
    /// Gets or sets the label text for the read/stop steps toggle button.
    /// </summary>
    [ObservableProperty]
    private string readButtonText = "Read Steps";

    /// <summary>
    /// Gets or sets the file path of the captured food photo.
    /// </summary>
    [ObservableProperty]
    private string? capturedPhotoPath;

    /// <summary>
    /// Gets or sets the list of ingredients recognized by AI vision from a captured photo.
    /// </summary>
    [ObservableProperty]
    private List<string> recognizedIngredients = new();

    /// <summary>
    /// Gets or sets a value indicating whether AI vision has recognized ingredients from a photo.
    /// </summary>
    [ObservableProperty]
    private bool hasRecognizedIngredients;

    /// <summary>
    /// Gets or sets a value indicating whether the recipe image URL failed to load.
    /// </summary>
    [ObservableProperty]
    private bool isImageLoadFailed;

    /// <summary>
    /// Gets or sets a value indicating whether the recipe has a valid image URL.
    /// </summary>
    [ObservableProperty]
    private bool hasImageUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeDetailViewModel"/> class with dependency injection.
    /// </summary>
    /// <param name="dataService">The data service for retrieving and modifying recipe data.</param>
    /// <param name="speechService">The speech service for text-to-speech functionality.</param>
    /// <param name="cameraService">The camera service for capturing food photos.</param>
    /// <param name="visionService">The vision service for AI-based ingredient recognition.</param>
    public RecipeDetailViewModel(IDataService dataService, ISpeechService speechService, ICameraService cameraService, IVisionService visionService)
    {
        _dataService = dataService;
        _speechService = speechService;
        _cameraService = cameraService;
        _visionService = visionService;
    }

    /// <summary>
    /// Applies query attributes from navigation to load the recipe specified by the "id" parameter.
    /// </summary>
    /// <param name="query">The dictionary of navigation query parameters.</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && int.TryParse(id?.ToString(), out var recipeId))
        {
            _ = LoadRecipeAsync(recipeId);
        }
    }

    /// <summary>
    /// Loads the full recipe details by its unique identifier, including image URL validation.
    /// </summary>
    /// <param name="recipeId">The unique identifier of the recipe to load.</param>
    [RelayCommand]
    private async Task LoadRecipeAsync(int recipeId)
    {
        IsBusy = true;
        try
        {
            var recipe = await _dataService.GetRecipeByIdAsync(recipeId);
            if (recipe is not null)
            {
                Recipe = recipe;
                HasImageUrl = !string.IsNullOrEmpty(recipe.ImageUrl);
                IsImageLoadFailed = false;

                if (HasImageUrl)
                {
                    _ = CheckImageAsync(recipe.ImageUrl);
                }
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Checks whether the recipe's image URL is reachable by sending a HEAD request.
    /// </summary>
    /// <param name="imageUrl">The URL of the image to validate.</param>
    private async Task CheckImageAsync(string imageUrl)
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, imageUrl));
            if (!response.IsSuccessStatusCode)
                IsImageLoadFailed = true;
        }
        catch
        {
            IsImageLoadFailed = true;
        }
    }

    /// <summary>
    /// Toggles the favorite status of the current recipe and triggers haptic feedback.
    /// </summary>
    [RelayCommand]
    private async Task ToggleDetailFavoriteAsync()
    {
        await _dataService.ToggleFavoriteAsync(Recipe.Id);
        Recipe.IsFavorite = !Recipe.IsFavorite;
        OnPropertyChanged(nameof(Recipe));

        try
        {
            HapticFeedback.Perform(HapticFeedbackType.LongPress);
            System.Diagnostics.Debug.WriteLine("[FoodLens] HapticFeedback.LongPress triggered (toggle favorite)");
            if (Recipe.IsFavorite)
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(100));
                System.Diagnostics.Debug.WriteLine("[FoodLens] Vibration.Vibrate triggered (added to favorites)");
            }
        }
        catch (FeatureNotSupportedException ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FoodLens] Haptic/Vibration not supported: {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FoodLens] Haptic/Vibration error: {ex.Message}");
        }
    }

    private CancellationTokenSource? _readCts;

    /// <summary>
    /// Reads all recipe instruction steps aloud using text-to-speech, or stops reading if already in progress.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the status text displayed during the food scanning process.
    /// </summary>
    [ObservableProperty]
    private string scanStatusText = "";

    /// <summary>
    /// Gets or sets a value indicating whether a food scan is currently in progress.
    /// </summary>
    [ObservableProperty]
    private bool isScanning;

    /// <summary>
    /// Captures a photo using the device camera and sends it to AI vision for ingredient recognition.
    /// </summary>
    [RelayCommand]
    private async Task CaptureAndRecognizeAsync()
    {
        IsScanning = true;
        ScanStatusText = "Opening camera...";
        IsBusy = true;
        try
        {
            var photoPath = await _cameraService.CapturePhotoAsync();
            if (string.IsNullOrEmpty(photoPath))
            {
                ScanStatusText = "Camera cancelled.";
                return;
            }

            CapturedPhotoPath = photoPath;
            ScanStatusText = "Photo captured. Analyzing ingredients with AI...";

            var bytes = await _cameraService.GetPhotoBytesAsync(photoPath);
            if (bytes is null)
            {
                ScanStatusText = "Failed to read photo.";
                return;
            }

            var ingredients = await _visionService.RecognizeIngredientsAsync(bytes);
            RecognizedIngredients = ingredients;
            HasRecognizedIngredients = ingredients.Count > 0;

            if (ingredients.Count > 0)
                ScanStatusText = $"Detected {ingredients.Count} ingredient(s)!";
            else
                ScanStatusText = "No ingredients detected. Try a clearer photo.";
        }
        catch (FeatureNotSupportedException)
        {
            ScanStatusText = "Camera not supported on this device.";
            await Shell.Current.DisplayAlert("Not Supported", "Camera is not available on this device.", "OK");
        }
        catch (PermissionException)
        {
            ScanStatusText = "Camera permission denied.";
            await Shell.Current.DisplayAlert("Permission Required", "Please grant camera permission to use this feature.", "OK");
        }
        catch (HttpRequestException ex)
        {
            ScanStatusText = "Network error. Check internet connection.";
            await Shell.Current.DisplayAlert("Network Error", $"Could not reach the AI service.\n\n{ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            ScanStatusText = $"Error: {ex.Message}";
            await Shell.Current.DisplayAlert("Error", $"An error occurred:\n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsScanning = false;
        }
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Navigates to the recipe edit page for the current recipe.
    /// </summary>
    [RelayCommand]
    private async Task EditRecipeAsync()
    {
        await Shell.Current.GoToAsync($"recipeedit?id={Recipe.Id}");
    }

    /// <summary>
    /// Prompts the user for confirmation and permanently deletes the current recipe.
    /// </summary>
    [RelayCommand]
    private async Task DeleteRecipeAsync()
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Delete Recipe",
            "Are you sure you want to delete this recipe? This action cannot be undone.",
            "Delete", "Cancel");

        if (!confirm) return;

        await _dataService.DeleteRecipeAsync(Recipe.Id);
        await Shell.Current.GoToAsync("../..");
    }
}
