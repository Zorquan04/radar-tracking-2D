namespace RadarTracking2D.Core.Tracking.Association;

public class AssociationNode
{
    public Hypothesis Hypothesis { get; }
    public double Probability { get; set; } = 1.0;
    public List<AssociationNode> Children { get; } = new();

    public AssociationNode(Hypothesis hypothesis)
    {
        Hypothesis = hypothesis;
    }

    public void AddChild(AssociationNode child)
    {
        Children.Add(child);
    }
}