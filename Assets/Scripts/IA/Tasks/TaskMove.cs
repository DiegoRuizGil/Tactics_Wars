using System;
using System.Collections.Generic;
using System.Linq;
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

            if (ManhattanDistance(_unit.transform.position, targetPosition) <= _unit.AttackRange
                    && Tree.GetData("target") != null) // only when target to attack is selected
            {
                _state = TreeNodeState.SUCCESS;
                return _state;
            }

            List<Node> aStarPath = AStarPathfinding.Instance.GetPath(
                    _unit.transform.position,
                    targetPosition,
                    _unit.Team
                );

            if (aStarPath.Count == 0)
            {
                _state = TreeNodeState.FAILURE;
                return _state;
            }

            if (aStarPath.Count <= _unit.AttackRange - 1
                 && Tree.GetData("target") != null) // only when target to attack is selected
            {
                _state = TreeNodeState.RUNNING;
                return _state;
            }

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
            //_unit.HasFinished = true;
            _state = TreeNodeState.SUCCESS;
            return _state;
        }
    }

    private List<Vector3> GetPathInRange(List<Node> aStarPath)
    {
        int limit = Math.Min(_unit.MovementRange, aStarPath.Count);

        List<Node> pathInRange = aStarPath.GetRange(0, limit);
        int pathCount = pathInRange.Count;

        for (int i = pathCount - 1; i >= 0; i--)
        {
            if (i < 0)
                Debug.Log($"Indice negativo: {i}");
            if (pathInRange[i].GetEntity(1) != null
                && pathInRange[i].GetEntity(1).Team == _unit.Team)
            {
                pathInRange.RemoveAt(i);
            }
            else
            {
                break;
            }
        }

        return pathInRange.Select(node => node.Position).ToList();
    }

    private int ManhattanDistance(Vector3 pos1, Vector3 pos2)
    {
        Node node1 = Grid.Instance.GetNode(pos1);
        Node node2 = Grid.Instance.GetNode(pos2);

        int xdistance = Math.Abs(node1.GridX - node2.GridX);
        int ydistance = Math.Abs(node1.GridY - node2.GridY);

        return xdistance + ydistance;
    }
}
