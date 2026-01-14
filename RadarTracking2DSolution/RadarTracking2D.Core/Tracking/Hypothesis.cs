namespace RadarTracking2D.Core.Tracking;

// single assignment hypothesis: which measurement goes to which track (or null)
public class Hypothesis
{
    public Dictionary<int, int?> Assignments { get; } = new(); // key: measurement index, value: trackId or null
    public double Probability { get; set; } = 1.0;

    public Hypothesis Clone()
    {
        var copy = new Hypothesis();
        foreach (var kv in Assignments)
            copy.Assignments[kv.Key] = kv.Value;
        copy.Probability = Probability;
        return copy;
    }
}