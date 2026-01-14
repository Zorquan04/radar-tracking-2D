using RadarTracking2D.Core.Statistics;
using RadarTracking2D.Core.Tracking;

namespace RadarTracking2D.Tests
{
    [TestFixture]
    public class TrackingTests
    {
        [Test]
        public void TrackCreation_ShouldAddNewTrack()
        {
            var tracker = new Tracker();
            var blobs = new[]
            {
                new BlobStatistics(new Core.Data.Blob(1) { Pixels = { (1,1), (2,2) } })
            }.ToList();

            tracker.ProcessFrame(blobs);
            var tracks = tracker.GetTracks();

            Assert.That(tracks.Count, Is.EqualTo(1));
            Assert.That(tracks[0].Id, Is.EqualTo(1));
        }

        [Test]
        public void TrackUpdate_ShouldUpdatePosition()
        {
            var tracker = new Tracker();
            var blob1 = new BlobStatistics(new Core.Data.Blob(1) { Pixels = { (1, 1), (2, 2) } });
            tracker.ProcessFrame(new[] { blob1 }.ToList());

            var blob2 = new BlobStatistics(new Core.Data.Blob(1) { Pixels = { (3, 3), (4, 4) } });
            tracker.ProcessFrame(new[] { blob2 }.ToList());

            var track = tracker.GetTracks().First();
            Assert.That(track.Distribution.MeanX, Is.EqualTo(blob2.MeanX));
            Assert.That(track.Distribution.MeanY, Is.EqualTo(blob2.MeanY));
            Assert.That(track.Age, Is.EqualTo(2));
        }

        [Test]
        public void EmptyFrame_ShouldNotThrow()
        {
            var tracker = new Tracker();
            tracker.ProcessFrame(new System.Collections.Generic.List<BlobStatistics>());
            Assert.Pass("Processing empty frame does not throw");
        }
    }
}