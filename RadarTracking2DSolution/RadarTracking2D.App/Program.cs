using RadarTracking2D.Core.Generation;
using RadarTracking2D.Core.ImageProcessing;

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

        Console.ReadKey();
    }
}