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
                probability *= 0.1; // penalization of the missing track
                continue;
            }

            var measurement = measurements[measurementIndex];
            var track = tracks.Find(t => t.Id == trackId.Value);

            double px = measurement.MeanX;
            double py = measurement.MeanY;

            probability *= track!.Distribution.Probability(px, py);
        }

        return probability;
    }
}