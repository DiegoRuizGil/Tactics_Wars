using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskFinalize : TreeNode
{
    private Unit _unit;

    public TaskFinalize(BehaviourTree.Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        _unit.HasFinished = true;

        _state = TreeNodeState.SUCCESS;
        return _state;
    }
}
