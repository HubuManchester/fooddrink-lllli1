namespace FoodLens.Services;

public interface ILocationService
{
    Task<Location?> GetCurrentLocationAsync();
    Task<string?> GetAddressAsync(double latitude, double longitude);
}
