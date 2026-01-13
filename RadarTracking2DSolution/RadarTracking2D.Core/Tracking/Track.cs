using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class Track(int id, GaussianDistribution distribution)
{
    public int Id { get; } = id;
    public GaussianDistribution Distribution { get; private set; } = distribution;
    public int Age { get; private set; } = 0;

    public void Update(GaussianDistribution newDistribution)
    {
        Distribution = newDistribution;
        Age++;
    }
}