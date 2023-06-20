using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : TreeNode
    {
        public Sequence(Tree tree) : base(tree) { }
        public Sequence(Tree tree, List<TreeNode> children) : base(tree, children) { }

        public override TreeNodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (TreeNode node in _children)
            {
                switch (node.Evaluate())
                {
                    case TreeNodeState.FAILURE:
                        _state = TreeNodeState.FAILURE;
                        return _state;
                    case TreeNodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    case TreeNodeState.SUCCESS:
                        continue;
                    default:
                        _state = TreeNodeState.SUCCESS;
                        return _state;
                }
            }

            _state = anyChildIsRunning ? TreeNodeState.RUNNING : TreeNodeState.SUCCESS;
            return _state;
        }
    }
}
