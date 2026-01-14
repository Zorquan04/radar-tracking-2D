using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking.Association;

public static class ProbabilityCalculator
{
    public static double Evaluate(Hypothesis hypothesis, List<BlobStatistics> measurements, List<Track> tracks)
    {
        double probability = 1.0; // start with full probability

        foreach (var kv in hypothesis.Assignments)
        {
            int measurementIndex = kv.Key;
            int? trackId = kv.Value;

            if (trackId == null)
            {
                probability *= 0.1; // measurement considered noise
                continue;
            }

            if (measurementIndex < 0 || measurementIndex >= measurements.Count)
            {
                probability *= 0.01; // out-of-bounds index
                continue;
            }

            var measurement = measurements[measurementIndex];
            var track = tracks.FirstOrDefault(t => t.Id == trackId.Value);

            if (track == null)
            {
                probability *= 0.01; // track disappeared
                continue;
            }

            double px = measurement.MeanX;
            double py = measurement.MeanY;

            probability *= track.Distribution.Probability(px, py); // multiply by Gaussian probability
        }

        return probability;
    }
}