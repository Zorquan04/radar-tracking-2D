namespace RadarTracking2D.Core.Tracking;

public class MhTreeNode(Hypothesis hypothesis)
{
    public Hypothesis Hypothesis { get; } = hypothesis;
    public double Probability { get; set; } = 1.0;
    public List<MhTreeNode> Children { get; } = new();

    public void AddChild(MhTreeNode child) => Children.Add(child);

    public IEnumerable<MhTreeNode> GetLeaves()
    {
        if (Children.Count == 0)
            yield return this;
        else
        {
            foreach (var child in Children)
            {
                foreach (var leaf in child.GetLeaves())
                    yield return leaf;
            }
        }
    }
}