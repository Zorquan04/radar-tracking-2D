using RadarTracking2D.Core.Segmentation;

namespace RadarTracking2D.Tests
{
    [TestFixture]
    public class CclTests
    {
        [Test]
        public void SingleBlob_ShouldBeDetected()
        {
            bool[,] image = new bool[5, 5];
            image[2, 2] = true;
            image[2, 3] = true;
            image[3, 2] = true;

            var ccl = new ConnectedComponentLabeling();
            var blobs = ccl.Process(image);

            Assert.That(blobs.Count, Is.EqualTo(1));
            Assert.That(blobs[0].Pixels.Count, Is.EqualTo(3));
        }

        [Test]
        public void TwoSeparateBlobs_ShouldBeDetected()
        {
            bool[,] image = new bool[5, 5];
            image[0, 0] = true;
            image[0, 1] = true;

            image[4, 4] = true;
            image[3, 4] = true;

            var ccl = new ConnectedComponentLabeling();
            var blobs = ccl.Process(image);

            Assert.That(blobs.Count, Is.EqualTo(2));
            var counts = blobs.Select(b => b.Pixels.Count).OrderBy(c => c).ToArray();
            Assert.That(counts, Is.EqualTo(new int[] { 2, 2 }));
        }

        [Test]
        public void EmptyImage_ShouldReturnNoBlobs()
        {
            bool[,] image = new bool[3, 3];
            var ccl = new ConnectedComponentLabeling();
            var blobs = ccl.Process(image);
            Assert.That(blobs.Count, Is.EqualTo(0));
        }
    }
}