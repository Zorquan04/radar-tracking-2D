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
        var extractor = new BlobExtractor();
        var tracker = new Tracker();

        int frameCount = 5;

        for (int f = 0; f < frameCount; f++)
        {
            var frame = generator.Generate(100, 100, objectCount: 2);
            int threshold = thresholding.CalculateThreshold(frame);
            var binary = thresholding.ApplyThreshold(frame, threshold);
            var blobs = extractor.Extract(binary);
            var blobStats = blobs.Select(b => new BlobStatistics(b)).ToList();

            tracker.ProcessFrame(blobStats);

            Console.WriteLine($"Frame {f + 1}");
            foreach (var t in tracker.GetTracks())
            {
                Console.WriteLine($"Track {t.Id}: meanX={t.Distribution.MeanX:F1}, meanY={t.Distribution.MeanY:F1}");
            }
        }
    }
}