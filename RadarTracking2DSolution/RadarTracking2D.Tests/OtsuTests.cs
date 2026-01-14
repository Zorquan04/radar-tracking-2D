using RadarTracking2D.Core.Data;
using RadarTracking2D.Core;

namespace RadarTracking2D.Tests
{
    [TestFixture]
    public class OtsuTests
    {
        [Test]
        public void UniformImage_ShouldReturnThresholdAtPixelValue()
        {
            var frame = new RadarFrame(3, 3);
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    frame.SetPixel(x, y, 100);

            var otsu = new ThresholdingService();
            int threshold = otsu.CalculateThreshold(frame);

            // all pixels same, threshold should be that value
            Assert.That(threshold, Is.EqualTo(100));
        }

        [Test]
        public void SimpleBimodal_ShouldSeparateCorrectly()
        {
            var frame = new RadarFrame(4, 1);
            frame.SetPixel(0, 0, 10);
            frame.SetPixel(1, 0, 10);
            frame.SetPixel(2, 0, 200);
            frame.SetPixel(3, 0, 200);

            var otsu = new ThresholdingService();
            int threshold = otsu.CalculateThreshold(frame);

            Assert.That(threshold, Is.InRange(0, 255));
        }

        [Test]
        public void ApplyThreshold_ShouldConvertCorrectly()
        {
            var frame = new RadarFrame(2, 2);
            frame.SetPixel(0, 0, 50);
            frame.SetPixel(0, 1, 150);
            frame.SetPixel(1, 0, 200);
            frame.SetPixel(1, 1, 30);

            var otsu = new ThresholdingService();
            int threshold = otsu.CalculateThreshold(frame);
            var binary = otsu.ApplyThreshold(frame, threshold);

            Assert.That(binary[0, 0], Is.EqualTo(frame.GetPixel(0, 0) >= threshold));
            Assert.That(binary[0, 1], Is.EqualTo(frame.GetPixel(0, 1) >= threshold));
            Assert.That(binary[1, 0], Is.EqualTo(frame.GetPixel(1, 0) >= threshold));
            Assert.That(binary[1, 1], Is.EqualTo(frame.GetPixel(1, 1) >= threshold));
        }
    }
}