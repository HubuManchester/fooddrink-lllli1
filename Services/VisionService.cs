namespace FoodLens.Services;

public class VisionService : IVisionService
{
    public async Task<List<string>> RecognizeIngredientsAsync(byte[] imageData)
    {
        try
        {
            await Task.Delay(500);
            return new List<string> { "Tomato", "Onion", "Garlic" };
        }
        catch (Exception)
        {
            return new List<string>();
        }
    }
}
