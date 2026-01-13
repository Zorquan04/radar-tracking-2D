namespace RadarTracking2D.Core.Tracking.Association;

public class AssociationTree
{
    public AssociationNode Root { get; }

    public AssociationTree(Hypothesis rootHypothesis)
    {
        Root = new AssociationNode(rootHypothesis);
    }

    public IEnumerable<AssociationNode> GetLeaves()
    {
        var stack = new Stack<AssociationNode>();
        stack.Push(Root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (node.Children.Count == 0)
                yield return node;
            else
                foreach (var child in node.Children)
                    stack.Push(child);
        }
    }
}