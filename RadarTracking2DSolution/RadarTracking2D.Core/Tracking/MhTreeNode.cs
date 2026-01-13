namespace RadarTracking2D.Core.Tracking;

public class MhTreeNode(Hypothesis hypothesis)
{
    public Hypothesis Hypothesis { get; } = hypothesis;
    public List<MhTreeNode> Children { get; } = new();

    public void AddChild(MhTreeNode child)
    {
        Children.Add(child);
    }
}