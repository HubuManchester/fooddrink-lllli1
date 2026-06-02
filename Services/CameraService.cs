namespace FoodLens.Services;

/// <summary>
/// Provides camera capture functionality using MediaPicker.
/// </summary>
public class CameraService : ICameraService
{
    /// <summary>
    /// Captures a photo using the device camera and returns the file path, or null on failure.
    /// </summary>
    public async Task<string?> CapturePhotoAsync()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
                return null;

            var photo = await MediaPicker.CapturePhotoAsync();
            return photo?.FullPath;
        }
        catch (FeatureNotSupportedException)
        {
            return null;
        }
        catch (PermissionException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Reads the contents of a photo file and returns it as a byte array, or null on failure.
    /// </summary>
    public async Task<byte[]?> GetPhotoBytesAsync(string filePath)
    {
        try
        {
            return await File.ReadAllBytesAsync(filePath);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
