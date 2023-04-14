using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskMove : TreeNode
{
    private Unit _unit;
    private BaseAction _action;

    public TaskMove(BehaviourTree.Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        if (_action == null)
        {
            // PREVIOS NODE HAS SET THE TARGET POSITION
            Vector3 targetPosition = (Tree.GetData("targetPosition") as Vector3?).Value;

            List<Vector3> path = AStarPathfinding.Instance.GetPath(
                    _unit.transform.position,
                    targetPosition,
                    _unit.Team
                );

            if (path.Count > _unit.MovementRange)
                path = path.GetRange(0, _unit.MovementRange);

            _action = new MoveAction(_unit, path, 2);
            _action.Execute();

            _state = TreeNodeState.RUNNING;
            return _state;
        }

        if (_action.IsRunning)
        {
            _state = TreeNodeState.RUNNING;
            return _state;
        }
        else
        {
            _action = null;
            _state = TreeNodeState.SUCCESS;
            return _state;
        }
    }
}
