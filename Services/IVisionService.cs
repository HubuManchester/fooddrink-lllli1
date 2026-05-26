namespace FoodLens.Services;

public interface IVisionService
{
    Task<List<string>> RecognizeIngredientsAsync(byte[] imageData);
}
