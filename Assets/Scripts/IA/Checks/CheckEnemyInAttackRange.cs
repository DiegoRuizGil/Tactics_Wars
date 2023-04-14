using System;
using UnityEngine;
using BehaviourTree;

public class CheckEnemyInAttackRange : TreeNode
{
    private Unit _unit;

    public CheckEnemyInAttackRange(BehaviourTree.Tree tree, Unit unit) : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        Entity target = Tree.GetData("target") as Entity;
        if (target == null)
        {
            Tree.ClearData("target");
            Tree.ClearData("targetPosition");
            _state = TreeNodeState.FAILURE;
            return _state;
        }

        int distanceFromTarget = CalculateDistance(_unit.transform.position, target.transform.position);
        if (distanceFromTarget > _unit.AttackRange)
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }

        _state = TreeNodeState.SUCCESS;
        return _state;
    }

    private int CalculateDistance(Vector3 initialPosition, Vector3 targetPosition)
    {
        Node initialNode = Grid.Instance.GetNode(initialPosition);
        Node targetNode = Grid.Instance.GetNode(targetPosition);

        return Math.Abs(initialNode.GridX - targetNode.GridX) + Math.Abs(initialNode.GridY - targetNode.GridY);
    }
}
