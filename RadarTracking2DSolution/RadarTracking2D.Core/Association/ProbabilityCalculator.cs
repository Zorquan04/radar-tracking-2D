using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking.Association;

public static class ProbabilityCalculator
{
    public static double Evaluate(Hypothesis hypothesis, List<BlobStatistics> measurements, List<Track> tracks)
    {
        double probability = 1.0;

        foreach (var kv in hypothesis.Assignments)
        {
            int measurementIndex = kv.Key;
            int? trackId = kv.Value;

            if (trackId == null)
            {
                probability *= 0.1; // disturbance
                continue;
            }

            if (measurementIndex < 0 || measurementIndex >= measurements.Count)
            {
                probability *= 0.01;
                continue;
            }

            var measurement = measurements[measurementIndex];
            var track = tracks.FirstOrDefault(t => t.Id == trackId.Value);

            if (track == null)
            {
                probability *= 0.01; // invalid / vanished track
                continue;
            }

            double px = measurement.MeanX;
            double py = measurement.MeanY;

            probability *= track.Distribution.Probability(px, py);
        }

        return probability;
    }
}