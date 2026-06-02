namespace FoodLens.Services;

/// <summary>
/// Provides accelerometer-based shake detection for the device.
/// </summary>
public interface IAccelerometerService : IDisposable
{
    /// <summary>
    /// Gets whether the device has an accelerometer sensor.
    /// </summary>
    bool IsSupported { get; }

    /// <summary>
    /// Gets whether the accelerometer is currently being monitored.
    /// </summary>
    bool IsMonitoring { get; }

    /// <summary>
    /// Starts monitoring the accelerometer for shake gestures.
    /// </summary>
    void StartMonitoring();

    /// <summary>
    /// Stops monitoring the accelerometer.
    /// </summary>
    void StopMonitoring();

    /// <summary>
    /// Event raised when a shake gesture is detected.
    /// </summary>
    event EventHandler? ShakeDetected;
}
