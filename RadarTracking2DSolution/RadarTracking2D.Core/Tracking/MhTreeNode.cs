using RadarTracking2D.Core.Tracking.Association;

namespace RadarTracking2D.Core.Tracking;

// node of MHT tree – basically wraps Hypothesis + children
public class MhTreeNode : AssociationNode
{
    public MhTreeNode(Hypothesis hypothesis) : base(hypothesis) { }

    // recursively return all leaves
    public IEnumerable<MhTreeNode> GetLeaves()
    {
        if (Children.Count == 0)
            yield return this;
        else
        {
            foreach (var child in Children)
            {
                if (child is MhTreeNode mhChild)
                {
                    foreach (var leaf in mhChild.GetLeaves())
                        yield return leaf;
                }
            }
        }
    }

    public void AddChild(MhTreeNode child) => base.AddChild(child);
}