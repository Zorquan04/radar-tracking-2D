using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking.Association;

public static class ProbabilityCalculator
{
    private const double MinProb = 0.1; // minimum probability of track
    private const double NoiseProb = 0.1;

    public static double Evaluate(Hypothesis hypothesis, List<BlobStatistics> measurements, List<Track> tracks)
    {
        double probability = 1.0;

        foreach (var kv in hypothesis.Assignments)
        {
            int measurementIndex = kv.Key;
            int? trackId = kv.Value;

            if (trackId == null)
            {
                probability *= NoiseProb;
                continue;
            }

            if (measurementIndex < 0 || measurementIndex >= measurements.Count)
            {
                probability *= MinProb; // out-of-bounds
                continue;
            }

            var measurement = measurements[measurementIndex];
            var track = tracks.FirstOrDefault(t => t.Id == trackId.Value);

            if (track == null)
            {
                probability *= MinProb; // the track disappeared
                continue;
            }

            double px = measurement.MeanX;
            double py = measurement.MeanY;

            // Gaussian probability for track
            double prob = track.Distribution.Probability(px, py);

            // gate minimum probability
            prob = Math.Max(prob, MinProb);

            probability *= prob;
        }

        return probability;
    }
}