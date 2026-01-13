using RadarTracking2D.Core.Tracking.Association;
using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class MhTree
{
    public MhTreeNode Root { get; } = new(new Hypothesis());

    public void Expand(List<BlobStatistics> measurements, List<Track> tracks, ref int nextTrackId)
    {
        var leaves = Root.GetLeaves().ToList();

        foreach (var leaf in leaves)
        {
            foreach (var mIndex in Enumerable.Range(0, measurements.Count))
            {
                // if there are no tracks (first frame), create a new track immediately
                if (tracks.Count == 0)
                {
                    int newId = nextTrackId++;
                    var newHyp = leaf.Hypothesis.Clone();
                    newHyp.Assignments[mIndex] = newId;

                    var child = new MhTreeNode(newHyp)
                    {
                        Probability = 1.0 // temporarily
                    };
                    leaf.AddChild(child);
                    continue;
                }

                // assignment to existing tracks
                foreach (var t in tracks)
                {
                    var newHyp = leaf.Hypothesis.Clone();
                    newHyp.Assignments[mIndex] = t.Id;

                    var child = new MhTreeNode(newHyp)
                    {
                        Probability = ProbabilityCalculator.Evaluate(newHyp, measurements, tracks)
                    };
                    leaf.AddChild(child);
                }

                // measurement as a disturbance
                var unassignedHyp = leaf.Hypothesis.Clone();
                unassignedHyp.Assignments[mIndex] = null;

                var unassignedChild = new MhTreeNode(unassignedHyp)
                {
                    Probability = ProbabilityCalculator.Evaluate(unassignedHyp, measurements, tracks)
                };
                leaf.AddChild(unassignedChild);

                // new track for measurement
                int newTrackId = nextTrackId++;
                var newTrackHyp = leaf.Hypothesis.Clone();
                newTrackHyp.Assignments[mIndex] = newTrackId;

                var newChild = new MhTreeNode(newTrackHyp)
                {
                    Probability = ProbabilityCalculator.Evaluate(newTrackHyp, measurements, tracks)
                };
                leaf.AddChild(newChild);
            }
        }
    }
}