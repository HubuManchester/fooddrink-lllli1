namespace FoodLens.Services;

/// <summary>
/// Provides AI-powered ingredient recognition from food images using YOLO models.
/// </summary>
public interface IVisionService
{
    /// <summary>
    /// Analyzes an image and returns a list of recognized food ingredient names.
    /// </summary>
    /// <param name="imageData">Raw image bytes to analyze.</param>
    /// <returns>List of recognized ingredient names.</returns>
    Task<List<string>> RecognizeIngredientsAsync(byte[] imageData);
}
