using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitBT : BehaviourTree.Tree
{
    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    protected override TreeNode SetupTree()
    {
        TreeNode root = new Sequence(this, new List<TreeNode>
        {
            new CheckCanDoActions(this, _unit),
            new Selector(this, new List<TreeNode>
            {
                new Sequence(this, new List<TreeNode>
                {
                    new CheckEnemyInAttackRange(this, _unit),
                    new TaskAttack(this, _unit)
                }),
                new Sequence(this, new List<TreeNode>
                {
                    new CheckTargetToAttack(this, _unit),
                    new TaskMove(this, _unit)
                }),
                new TaskFinalize(this, _unit)
            })
        });

        return root;
    }
}
