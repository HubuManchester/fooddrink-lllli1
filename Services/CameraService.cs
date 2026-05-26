namespace FoodLens.Services;

public class CameraService : ICameraService
{
    public async Task<string?> CapturePhotoAsync()
    {
        try
        {
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
