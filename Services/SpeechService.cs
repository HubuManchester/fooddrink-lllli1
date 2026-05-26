using System.Threading;

namespace FoodLens.Services;

public class SpeechService : ISpeechService
{
    private CancellationTokenSource? _cts;

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

    public Task StopAsync()
    {
        _cts?.Cancel();
        return Task.CompletedTask;
    }
}
