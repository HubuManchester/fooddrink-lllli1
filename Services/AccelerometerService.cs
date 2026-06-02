using System.Diagnostics;

namespace FoodLens.Services;

/// <summary>
/// Provides shake detection using the device accelerometer sensor.
/// </summary>
public class AccelerometerService : IAccelerometerService
{
    private bool _disposed;

    /// <summary>
    /// Gets whether the accelerometer sensor is available on this device.
    /// </summary>
    public bool IsSupported => Accelerometer.IsSupported;
    /// <summary>
    /// Gets whether the accelerometer is currently being monitored for shake events.
    /// </summary>
    public bool IsMonitoring => Accelerometer.IsMonitoring;

    /// <summary>
    /// Event raised when a device shake is detected.
    /// </summary>
    public event EventHandler? ShakeDetected;

    /// <summary>
    /// Starts monitoring the accelerometer for shake gestures.
    /// </summary>
    public void StartMonitoring()
    {
        if (!IsSupported || IsMonitoring) return;

        try
        {
            Accelerometer.ShakeDetected += OnShakeDetected;
            Accelerometer.Start(SensorSpeed.Game);
            Debug.WriteLine("[FoodLens] Accelerometer shake monitoring started");
        }
        catch (FeatureNotSupportedException ex)
        {
            Debug.WriteLine($"[FoodLens] Accelerometer not supported: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[FoodLens] Accelerometer error: {ex.Message}");
        }
    }

    /// <summary>
    /// Stops monitoring the accelerometer for shake gestures.
    /// </summary>
    public void StopMonitoring()
    {
        if (!IsMonitoring) return;

        try
        {
            Accelerometer.ShakeDetected -= OnShakeDetected;
            Accelerometer.Stop();
            Debug.WriteLine("[FoodLens] Accelerometer shake monitoring stopped");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[FoodLens] Accelerometer stop error: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the platform shake-detected event and raises the ShakeDetected event.
    /// </summary>
    private void OnShakeDetected(object? sender, EventArgs e)
    {
        Debug.WriteLine("[FoodLens] Shake detected!");
        ShakeDetected?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Releases resources by stopping accelerometer monitoring.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        StopMonitoring();
        _disposed = true;
    }
}
