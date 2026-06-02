using System.Threading;

#if ANDROID
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Content.PM;
using Android.Util;
using Android.Media;
#endif

#if WINDOWS
using Windows.Media.SpeechRecognition;
using System.Diagnostics;
#endif

using System.Text.Json;

namespace FoodLens.Services;

public class SpeechRecognitionService : ISpeechRecognitionService
{
    const string TAG = "FoodLens";

    const string BaiduApiKey = "YOUR_BAIDU_API_KEY";
    const string BaiduSecretKey = "YOUR_BAIDU_SECRET_KEY";

#if ANDROID
    public bool IsSupported => true;
#elif WINDOWS
    public bool IsSupported => true;
#else
    public bool IsSupported => false;
#endif

    public async Task<string?> ListenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
#if ANDROID
            return await ListenAndroidAsync(cancellationToken);
#elif WINDOWS
            return await ListenWindowsAsync(cancellationToken);
#else
            return null;
#endif
        }
        catch (Exception ex)
        {
#if ANDROID
            Log.Debug(TAG, $"ListenAsync error: {ex.GetType().Name}: {ex.Message}");
#elif WINDOWS
            System.Diagnostics.Debug.WriteLine($"[FoodLens] ListenAsync error: {ex.GetType().Name}: {ex.Message}");
#endif
            throw;
        }
    }

#if ANDROID
    private async Task<string?> ListenAndroidAsync(CancellationToken ct)
    {
        var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
        if (activity == null)
        {
            Log.Debug(TAG, "Activity is null");
            return null;
        }

        var status = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.Microphone>();
        if (status != Microsoft.Maui.ApplicationModel.PermissionStatus.Granted)
        {
            Log.Debug(TAG, "Microphone permission denied");
            return null;
        }

        try
        {
            return await ListenViaIntentAsync(activity, ct);
        }
        catch (ActivityNotFoundException)
        {
            Log.Debug(TAG, "RecognizerIntent not available");
        }

        var service = FindRecognitionService(activity);
        if (service != null)
        {
            var result = await TrySpeechRecognizerAsync(activity, service, ct);
            if (result != null) return result;
        }

        Log.Debug(TAG, "Falling back to cloud speech recognition...");
        return await ListenViaCloudAsync(ct);
    }

    private async Task<string?> ListenViaIntentAsync(Android.App.Activity activity, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<string?>();
        MainActivity.SpeechResultTcs = tcs;

        var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
        intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
        intent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");
        intent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now...");

        activity.StartActivityForResult(intent, MainActivity.SpeechRequestCode);

        using var registration = ct.Register(() => tcs.TrySetResult(null));
        return await tcs.Task;
    }

    private ComponentName? FindRecognitionService(Android.App.Activity activity)
    {
        var pm = activity.PackageManager!;

        var services = pm.QueryIntentServices(
            new Intent("android.speech.RecognitionService"),
            PackageInfoFlags.MatchAll);

        if (services != null)
        {
            foreach (var info in services)
            {
                var pkg = info.ServiceInfo.PackageName;
                if (pkg.Contains("market") || pkg.Contains("store") || pkg.Contains("install") || pkg.Contains("tencent"))
                    continue;
                Log.Debug(TAG, $"Found RecognitionService: {pkg}/{info.ServiceInfo.Name}");
                return new ComponentName(pkg, info.ServiceInfo.Name);
            }
        }

        Log.Debug(TAG, "QueryIntentServices found nothing useful, scanning Google packages...");

        string[] googlePkgs = ["com.google.android.googlequicksearchbox", "com.google.android.gms"];
        foreach (var pkgName in googlePkgs)
        {
            try
            {
                var pkgInfo = pm.GetPackageInfo(pkgName, PackageInfoFlags.Services);
                if (pkgInfo?.Services != null)
                {
                    foreach (var si in pkgInfo.Services)
                    {
                        var name = si.Name.ToLowerInvariant();
                        Log.Debug(TAG, $"  {pkgName}: {si.Name}");
                        if (name.Contains("recognition") || name.Contains("speechrecog"))
                        {
                            Log.Debug(TAG, $"  -> Match: {pkgName}/{si.Name}");
                            return new ComponentName(pkgName, si.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, $"  {pkgName}: {ex.Message}");
            }
        }

        return null;
    }

    private async Task<string?> TrySpeechRecognizerAsync(Android.App.Activity activity, ComponentName service, CancellationToken ct)
    {
        try
        {
            return await ListenViaSpeechRecognizerAsync(activity, service, ct);
        }
        catch (Exception ex)
        {
            Log.Debug(TAG, $"SpeechRecognizer failed: {ex.Message}");
            return null;
        }
    }

    private class RetryableException : Exception { public RetryableException(string msg) : base(msg) { } }

    private async Task<string?> ListenViaSpeechRecognizerAsync(Android.App.Activity activity, ComponentName service, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<string?>();
        SpeechRecognizer? recognizer = null;

        try
        {
            recognizer = SpeechRecognizer.CreateSpeechRecognizer(activity, service);

            var listener = new SpeechRecognitionListener(
                results => tcs.TrySetResult(results),
                error =>
                {
                    Log.Debug(TAG, $"SpeechRecognizer error: {error}");
                    if (error.Contains("TooManyRequests") || error.Contains("ServerError"))
                        tcs.TrySetException(new RetryableException(error));
                    else
                        tcs.TrySetResult(null);
                });

            recognizer.SetRecognitionListener(listener);

            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            intent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");
            intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            recognizer.StartListening(intent);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            using var reg1 = cts.Token.Register(() =>
            {
                try { recognizer.StopListening(); } catch { }
                tcs.TrySetResult(null);
            });
            using var reg2 = ct.Register(() =>
            {
                try { recognizer.StopListening(); } catch { }
                tcs.TrySetResult(null);
            });

            return await tcs.Task;
        }
        finally
        {
            recognizer?.Dispose();
        }
    }

    private async Task<string?> ListenViaCloudAsync(CancellationToken ct)
    {
        if (string.IsNullOrEmpty(BaiduApiKey) || BaiduApiKey == "YOUR_BAIDU_API_KEY")
        {
            Log.Debug(TAG, "Baidu API key not configured, using direct recording + recognition");
            return await RecordAndRecognizeAsync(ct);
        }

        try
        {
            var token = await GetBaiduTokenAsync();
            if (token == null)
            {
                Log.Debug(TAG, "Failed to get Baidu token");
                return null;
            }

            var audioData = await RecordAudioAsync(ct);
            if (audioData == null || audioData.Length == 0)
            {
                Log.Debug(TAG, "No audio recorded");
                return null;
            }

            return await RecognizeViaBaiduAsync(token, audioData);
        }
        catch (Exception ex)
        {
            Log.Debug(TAG, $"Cloud recognition error: {ex.Message}");
            return null;
        }
    }

    private async Task<string?> RecordAndRecognizeAsync(CancellationToken ct)
    {
        var audioData = await RecordAudioAsync(ct);
        if (audioData == null || audioData.Length == 0)
        {
            Log.Debug(TAG, "No audio recorded");
            return null;
        }

        var token = await GetBaiduTokenAsync();
        if (token == null) return null;

        return await RecognizeViaBaiduAsync(token, audioData);
    }

    private async Task<byte[]?> RecordAudioAsync(CancellationToken ct)
    {
        const int sampleRate = 16000;
        const ChannelIn channel = ChannelIn.Mono;
        const Encoding AndroidFormat = Encoding.Pcm16bit;
        const int bufferSize = 3200;

        var minBuffer = AudioRecord.GetMinBufferSize(sampleRate, channel, AndroidFormat);
        var actualBuffer = System.Math.Max(bufferSize * 4, minBuffer);

        AudioRecord? recorder = null;
        try
        {
            recorder = new AudioRecord(AudioSource.Mic, sampleRate, channel, AndroidFormat, actualBuffer);
            if (recorder.State != State.Initialized)
            {
                Log.Debug(TAG, "AudioRecord not initialized");
                return null;
            }

            recorder.StartRecording();
            Log.Debug(TAG, "Recording started... speak now");

            using var ms = new MemoryStream();
            var buffer = new byte[bufferSize];
            var totalSilenceMs = 0;
            const int maxSilenceMs = 2000;
            const int maxDurationMs = 10000;
            var totalMs = 0;

            while (!ct.IsCancellationRequested && totalMs < maxDurationMs)
            {
                var read = await recorder.ReadAsync(buffer, 0, buffer.Length);
                if (read > 0)
                {
                    ms.Write(buffer, 0, read);
                    totalMs += bufferSize * 1000 / sampleRate;

                    short max = 0;
                    for (int i = 0; i < read - 1; i += 2)
                    {
                        short sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                        if (System.Math.Abs(sample) > max) max = System.Math.Abs(sample);
                    }

                    if (max < 300)
                    {
                        totalSilenceMs += bufferSize * 1000 / sampleRate;
                        if (totalSilenceMs > maxSilenceMs && ms.Length > 8000)
                        {
                            Log.Debug(TAG, "Silence detected, stopping");
                            break;
                        }
                    }
                    else
                    {
                        totalSilenceMs = 0;
                    }
                }
            }

            recorder.Stop();
            Log.Debug(TAG, $"Recording finished: {ms.Length} bytes, {totalMs}ms");

            if (ms.Length < 3200)
            {
                Log.Debug(TAG, "Recording too short");
                return null;
            }

            return ms.ToArray();
        }
        catch (Exception ex)
        {
            Log.Debug(TAG, $"Recording error: {ex.Message}");
            return null;
        }
        finally
        {
            recorder?.Release();
            recorder?.Dispose();
        }
    }

    private static string? _cachedToken;
    private static DateTime _tokenExpiry = DateTime.MinValue;

    private async Task<string?> GetBaiduTokenAsync()
    {
        if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry)
            return _cachedToken;

        try
        {
            using var client = new HttpClient();
            var url = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={BaiduApiKey}&client_secret={BaiduSecretKey}";
            var response = await client.GetStringAsync(url);
            using var doc = JsonDocument.Parse(response);

            if (doc.RootElement.TryGetProperty("access_token", out var tokenEl))
            {
                _cachedToken = tokenEl.GetString();
                _tokenExpiry = DateTime.UtcNow.AddHours(24);
                Log.Debug(TAG, "Baidu token obtained");
                return _cachedToken;
            }

            Log.Debug(TAG, $"Baidu token response: {response}");
            return null;
        }
        catch (Exception ex)
        {
            Log.Debug(TAG, $"Baidu token error: {ex.Message}");
            return null;
        }
    }

    private async Task<string?> RecognizeViaBaiduAsync(string token, byte[] audioData)
    {
        try
        {
            var base64 = Convert.ToBase64String(audioData);
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var body = new
            {
                format = "pcm",
                rate = 16000,
                channel = 1,
                cuid = "foodlens",
                token = token,
                speech = base64,
                len = audioData.Length
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://vop.baidu.com/server_api", content);
            var result = await response.Content.ReadAsStringAsync();

            Log.Debug(TAG, $"Baidu response: {result}");

            using var doc = JsonDocument.Parse(result);
            var root = doc.RootElement;

            var errNo = root.TryGetProperty("err_no", out var errEl) ? errEl.GetInt32() : -1;
            if (errNo == 0 && root.TryGetProperty("result", out var resultEl))
            {
                var text = resultEl.EnumerateArray().FirstOrDefault().GetString();
                Log.Debug(TAG, $"Recognized: {text}");
                return text;
            }

            var errMsg = root.TryGetProperty("err_msg", out var msgEl) ? msgEl.GetString() : "unknown";
            Log.Debug(TAG, $"Baidu error: {errNo} - {errMsg}");
            return null;
        }
        catch (Exception ex)
        {
            Log.Debug(TAG, $"Baidu recognition error: {ex.Message}");
            return null;
        }
    }

    private class SpeechRecognitionListener : Java.Lang.Object, IRecognitionListener
    {
        private readonly Action<string?> _onResults;
        private readonly Action<string> _onError;

        public SpeechRecognitionListener(Action<string?> onResults, Action<string> onError)
        {
            _onResults = onResults;
            _onError = onError;
        }

        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            _onResults(matches?.FirstOrDefault());
        }

        public void OnError(Android.Speech.SpeechRecognizerError error)
        {
            _onError(error.ToString());
        }

        public void OnReadyForSpeech(Bundle? params_) { }
        public void OnBeginningOfSpeech() { }
        public void OnRmsChanged(float rmsdB) { }
        public void OnBufferReceived(byte[]? buffer) { }
        public void OnEndOfSpeech() { }
        public void OnPartialResults(Bundle? partialResults) { }
        public void OnEvent(int eventType, Bundle? params_) { }
    }
#endif

#if WINDOWS
    private async Task<string?> ListenWindowsAsync(CancellationToken ct)
    {
        System.Diagnostics.Debug.WriteLine("[FoodLens] SpeechRecognition: Starting Windows speech recognition...");
        SpeechRecognizer? recognizer = null;
        try
        {
            recognizer = new SpeechRecognizer();
            await recognizer.CompileConstraintsAsync();
            var result = await recognizer.RecognizeAsync();
            if (result.Status == SpeechRecognitionResultStatus.Success)
                return result.Text;
            throw new InvalidOperationException($"Speech recognition failed: {result.Status}");
        }
        catch (Exception ex) when (ex.Message.Contains("privacy policy", StringComparison.OrdinalIgnoreCase) || ex.HResult == unchecked((int)0x80045509))
        {
            throw new InvalidOperationException(
                "Windows speech recognition is not enabled.\n\n" +
                "Please go to:\n" +
                "Settings > Privacy & security > Speech\n" +
                "and turn ON 'Online speech recognition'.");
        }
        finally
        {
            recognizer?.Dispose();
        }
    }
#endif
}
