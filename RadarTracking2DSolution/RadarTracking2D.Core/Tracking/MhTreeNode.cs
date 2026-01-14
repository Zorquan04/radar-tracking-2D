using RadarTracking2D.Core.Tracking.Association;

namespace RadarTracking2D.Core.Tracking;

public class MhTreeNode : AssociationNode
{
    public MhTreeNode(Hypothesis hypothesis) : base(hypothesis) { }

    // we add a convenient recursive method to the leaves
    public IEnumerable<MhTreeNode> GetLeaves()
    {
        if (Children.Count == 0)
            yield return this;
        else
        {
            foreach (var child in Children)
            {
                // we project children onto MhTreeNode – all children in MhTree are of this type
                if (child is MhTreeNode mhChild)
                {
                    foreach (var leaf in mhChild.GetLeaves())
                        yield return leaf;
                }
            }
        }
    }

    // we add a convenient child addition by casting to MhTreeNode
    public void AddChild(MhTreeNode child) => base.AddChild(child);
}