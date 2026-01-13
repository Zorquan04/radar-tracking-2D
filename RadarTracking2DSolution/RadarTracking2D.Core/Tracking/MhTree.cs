namespace RadarTracking2D.Core.Tracking;

public class MhTree
{
    public MhTreeNode Root { get; } = new(new Hypothesis());

    // adding new measurements -> developing hypotheses
    public void Expand(List<int> measurements, List<Track> tracks)
    {
        // placeholder – branching logic to be implemented in Tracker.cs
    }
}