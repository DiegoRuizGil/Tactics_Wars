using System.Collections.Generic;

namespace BehaviourTree
{
    public enum TreeNodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class TreeNode
    {
        protected TreeNodeState _state;

        public TreeNode parent;
        protected List<TreeNode> _children = new List<TreeNode>();

        private Tree _tree;
        public Tree Tree { get { return _tree; } }

        public TreeNode(Tree tree)
        {
            parent = null;
            _tree = tree;
        }

        public TreeNode(Tree tree, List<TreeNode> children)
        {
            _tree = tree;
            foreach (TreeNode child in children)
            {
                Attach(child);
            }
        }

        private void Attach(TreeNode node)
        {
            node.parent = this;
            _children.Add(node);
        }

        public virtual TreeNodeState Evaluate() => TreeNodeState.FAILURE;
    }
}
