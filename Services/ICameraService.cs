namespace FoodLens.Services;

/// <summary>
/// Provides camera capture functionality for taking photos.
/// </summary>
public interface ICameraService
{
    /// <summary>
    /// Opens the camera and captures a photo.
    /// </summary>
    /// <returns>The file path of the captured photo, or null if cancelled.</returns>
    Task<string?> CapturePhotoAsync();

    /// <summary>
    /// Reads a photo file as raw bytes.
    /// </summary>
    /// <param name="filePath">The path to the photo file.</param>
    /// <returns>The image bytes, or null if the file could not be read.</returns>
    Task<byte[]?> GetPhotoBytesAsync(string filePath);
}
