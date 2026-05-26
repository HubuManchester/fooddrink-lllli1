namespace FoodLens.Services;

public interface ISpeechService
{
    Task SpeakAsync(string text);
    Task StopAsync();
}
