namespace FoodLens.Services;

/// <summary>
/// Provides text-to-speech functionality for reading recipe steps aloud.
/// </summary>
public interface ISpeechService
{
    /// <summary>
    /// Speaks the given text aloud using the system TTS engine.
    /// </summary>
    /// <param name="text">The text to speak.</param>
    Task SpeakAsync(string text);

    /// <summary>
    /// Stops any ongoing speech playback.
    /// </summary>
    Task StopAsync();
}
