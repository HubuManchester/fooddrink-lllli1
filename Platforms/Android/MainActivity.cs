using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Speech;

namespace FoodLens;

/// <summary>
/// Android main activity that handles speech recognition results from RecognizerIntent.
/// </summary>
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    /// <summary>
    /// Request code used to identify speech recognition activity results.
    /// </summary>
    public const int SpeechRequestCode = 101;

    /// <summary>
    /// Task completion source that receives the speech recognition result text.
    /// </summary>
    public static System.Threading.Tasks.TaskCompletionSource<string?>? SpeechResultTcs { get; set; }

    /// <summary>
    /// Handles activity results, specifically speech recognition results from RecognizerIntent.
    /// </summary>
    /// <param name="requestCode">The request code identifying the activity.</param>
    /// <param name="resultCode">The result code from the activity.</param>
    /// <param name="data">The result data intent containing speech text.</param>
    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (requestCode == SpeechRequestCode)
        {
            if (resultCode == Result.Ok && data != null)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                SpeechResultTcs?.TrySetResult(matches?.FirstOrDefault());
            }
            else
            {
                SpeechResultTcs?.TrySetResult(null);
            }
        }
    }
}
