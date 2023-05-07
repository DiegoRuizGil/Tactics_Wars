using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskMove : TreeNode
{
    private readonly Unit _unit;
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

            List<Node> aStarPath = AStarPathfinding.Instance.GetPath(
                    _unit.transform.position,
                    targetPosition,
                    _unit.Team
                );

            List<Vector3> path = GetPathInRange(aStarPath);

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

    private List<Vector3> GetPathInRange(List<Node> aStarPath)
    {
        List<Vector3> pathInRange = new List<Vector3>();

        for (int i=0; i < _unit.MovementRange; i++)
        {
            if (i >= aStarPath.Count)
                break;

            if (aStarPath[i].GetTopEntity() != null &&
                aStarPath[i].GetTopEntity().Team == _unit.Team)
                continue;

            pathInRange.Add(aStarPath[i].Position);
        }

        return pathInRange;
    }
}
