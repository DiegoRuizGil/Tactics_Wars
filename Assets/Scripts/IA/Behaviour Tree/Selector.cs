using System.Collections.Generic;

namespace BehaviourTree
{
    public class Selector : TreeNode
    {
        public Selector(Tree tree) : base(tree) { }
        public Selector(Tree tree, List<TreeNode> children) : base(tree, children) { }

        public override TreeNodeState Evaluate()
        {

            foreach (TreeNode node in _children)
            {
                switch (node.Evaluate())
                {
                    case TreeNodeState.FAILURE:
                        continue;
                    case TreeNodeState.RUNNING:
                        _state = TreeNodeState.RUNNING;
                        return _state;
                    case TreeNodeState.SUCCESS:
                        _state = TreeNodeState.SUCCESS;
                        return _state;
                    default:
                        continue;
                }
            }

            _state = TreeNodeState.FAILURE;
            return _state;
        }
    }
}
