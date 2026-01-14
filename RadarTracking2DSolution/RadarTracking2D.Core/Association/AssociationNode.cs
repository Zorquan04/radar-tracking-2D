namespace RadarTracking2D.Core.Tracking.Association;

public class AssociationNode
{
    public Hypothesis Hypothesis { get; } // hypothesis for this node (which measurement goes to which track)
    public double Probability { get; set; } = 1.0; // probability of this hypothesis
    public List<AssociationNode> Children { get; } = new(); // possible next hypotheses

    public AssociationNode(Hypothesis hypothesis)
    {
        Hypothesis = hypothesis; // assign the hypothesis to this node
    }

    public void AddChild(AssociationNode child)
    {
        Children.Add(child); // add a child node representing next step in the association
    }
}
