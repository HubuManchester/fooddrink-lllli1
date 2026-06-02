namespace FoodLens.Services;

/// <summary>
/// Provides speech-to-text functionality for voice input in recipe forms.
/// </summary>
public interface ISpeechRecognitionService
{
    /// <summary>
    /// Starts listening for speech input and returns the recognized text.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the listening operation.</param>
    /// <returns>The recognized text, or null if recognition failed or was cancelled.</returns>
    Task<string?> ListenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets whether speech recognition is supported on the current platform.
    /// </summary>
    bool IsSupported { get; }
}
