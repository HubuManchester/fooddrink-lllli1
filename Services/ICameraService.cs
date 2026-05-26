namespace FoodLens.Services;

public interface ICameraService
{
    Task<string?> CapturePhotoAsync();
    Task<byte[]?> GetPhotoBytesAsync(string filePath);
}
