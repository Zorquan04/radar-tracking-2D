namespace RadarTracking2D.Core.Tracking;

public class Hypothesis
{
    public Dictionary<int, int?> Assignments { get; } = new(); 
    
    // key: measurement index, value: trackId or null if disruption
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