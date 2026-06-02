using System.Threading;

namespace FoodLens.Services;

/// <summary>
/// Provides text-to-speech functionality using the system TTS engine.
/// </summary>
public class SpeechService : ISpeechService
{
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Speaks the specified text aloud using the system text-to-speech engine, cancelling any previous speech.
    /// </summary>
    public async Task SpeakAsync(string text)
    {
        try
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            await TextToSpeech.SpeakAsync(text, cancelToken: _cts.Token);
        }
        catch (FeatureNotSupportedException)
        {
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Stops any in-progress text-to-speech playback.
    /// </summary>
    public Task StopAsync()
    {
        _cts?.Cancel();
        return Task.CompletedTask;
    }
}
