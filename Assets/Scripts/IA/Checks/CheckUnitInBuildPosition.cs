using UnityEngine;
using BehaviourTree;

public class CheckUnitInBuildPosition : TreeNode
{
    private readonly Unit _unit;

    public CheckUnitInBuildPosition(BehaviourTree.Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        Vector3? buildPosition = Tree.GetData("targetPosition") as Vector3?;
        if (buildPosition == null)
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }
        else if (Tree.GetData("target") == null) // check if unit task is not attack
        {
            if (Grid.Instance.CheckIfSameNode(buildPosition.Value, _unit.transform.position))
            {
                _state = TreeNodeState.SUCCESS;
                return _state;
            }
            else
            {
                _state = TreeNodeState.FAILURE;
                return _state;
            }
        }
        else
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }
    }
}
