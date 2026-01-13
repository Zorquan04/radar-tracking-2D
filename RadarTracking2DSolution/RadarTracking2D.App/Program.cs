using RadarTracking2D.Core.Generation;
using RadarTracking2D.Core.ImageProcessing;
using RadarTracking2D.Core.Segmentation;
using RadarTracking2D.Core.Statistics;
using RadarTracking2D.Core.Tracking;

namespace RadarTracking2D.App;

static class Program
{
    static void Main()
    {
        var generator = new RadarFrameGenerator();
        var thresholding = new ThresholdingService();

        var frame = generator.Generate(width: 100, height: 100, objectCount: 2);

        int threshold = thresholding.CalculateThreshold(frame);

        var binary = thresholding.ApplyThreshold(frame, threshold);

        Console.WriteLine($"Otsu threshold = {threshold}");
        
        for (int y = 0; y < frame.Height; y++)
        {
            for (int x = 0; x < frame.Width; x++)
            {
                Console.Write(binary[y, x] ? "#" : ".");
            }
            Console.WriteLine();
        }
        
        var extractor = new BlobExtractor();
        var blobs = extractor.Extract(binary);

        Console.WriteLine($"Blobs detected: {blobs.Count}");
        
        var tracker = new Tracker();
        var blobStats = blobs.Select(b => new BlobStatistics(b)).ToList();

        foreach (var stats in blobStats)
        {
            Console.WriteLine($"Blob {stats.BlobLabel}: pixels={stats.PixelCount}, meanX={stats.MeanX:F1}, meanY={stats.MeanY:F1}, stdX={stats.StdDevX:F1}, stdY={stats.StdDevY:F1}");
        }

        tracker.ProcessFrame(blobStats);
    }
}