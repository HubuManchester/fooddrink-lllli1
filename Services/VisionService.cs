using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace FoodLens.Services;

public class VisionService : IVisionService
{
    private InferenceSession? _session;
    private string[] _labels = [];
    private readonly object _lock = new();

    private void EnsureSessionLoaded()
    {
        lock (_lock)
        {
            if (_session != null) return;

            var modelPath = Path.Combine(FileSystem.AppDataDirectory, "food101_model.onnx");

            if (!File.Exists(modelPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("food101_model.onnx").GetAwaiter().GetResult();
                using var fs = new FileStream(modelPath, FileMode.Create, FileAccess.Write);
                stream.CopyTo(fs);
            }

            var sessionOptions = new SessionOptions();
            sessionOptions.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
            _session = new InferenceSession(modelPath, sessionOptions);
            _labels = LoadLabels();
        }
    }

    private static string[] LoadLabels()
    {
        var labels = new List<string>();
        try
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("food101_labels.txt").GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    labels.Add(line);
            }
        }
        catch
        {
            for (int i = 0; i < 101; i++)
                labels.Add($"food_{i}");
        }
        return labels.ToArray();
    }

    public async Task<bool> IsServerAvailableAsync()
    {
        try
        {
            EnsureSessionLoaded();
            return _session != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<string>> RecognizeIngredientsAsync(byte[] imageData)
    {
        EnsureSessionLoaded();

        var inputTensor = await PreprocessImageAsync(imageData);
        var inputName = _session!.InputNames.First();

        using var results = _session.Run(new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
        });

        var logits = results.First().AsEnumerable<float>().ToArray();
        var exps = logits.Select(x => MathF.Exp(x - logits.Max())).ToArray();
        var sum = exps.Sum();
        var probabilities = exps.Select(e => e / sum).ToArray();

        var topResults = probabilities
            .Select((prob, index) => (prob, index))
            .OrderByDescending(x => x.prob)
            .Take(1)
            .Where(x => x.prob > 0.01f)
            .ToList();

        if (topResults.Count == 0)
            return new List<string>();

        return topResults.Select(x =>
        {
            var label = x.index < _labels.Length ? _labels[x.index] : $"unknown_{x.index}";
            return $"{FormatLabel(label)} ({x.prob:P0})";
        }).ToList();
    }

    private static async Task<Tensor<float>> PreprocessImageAsync(byte[] imageData)
    {
        using var bmp = SkiaSharp.SKBitmap.Decode(imageData);
        if (bmp == null)
            throw new InvalidOperationException("Failed to decode image");

        var resized = new SkiaSharp.SKBitmap(224, 224);
        using var canvas = new SkiaSharp.SKCanvas(resized);
        using var paint = new SkiaSharp.SKPaint { FilterQuality = SkiaSharp.SKFilterQuality.High };
        canvas.DrawBitmap(bmp, new SkiaSharp.SKRect(0, 0, 224, 224), paint);

        var tensor = new DenseTensor<float>(new[] { 1, 3, 224, 224 });
        float[] mean = [0.485f, 0.456f, 0.406f];
        float[] std = [0.229f, 0.224f, 0.225f];

        for (int y = 0; y < 224; y++)
        {
            for (int x = 0; x < 224; x++)
            {
                var pixel = resized.GetPixel(x, y);
                tensor[0, 0, y, x] = (pixel.Red / 255f - mean[0]) / std[0];
                tensor[0, 1, y, x] = (pixel.Green / 255f - mean[1]) / std[1];
                tensor[0, 2, y, x] = (pixel.Blue / 255f - mean[2]) / std[2];
            }
        }

        return tensor;
    }

    private static string FormatLabel(string label)
    {
        if (string.IsNullOrEmpty(label)) return label;
        return string.Join(" ", label.Split('_').Select(w =>
            string.IsNullOrEmpty(w) ? w : char.ToUpper(w[0]) + w[1..].ToLower()));
    }
}
