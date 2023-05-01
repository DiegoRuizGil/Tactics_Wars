using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public class VillagerBT : BehaviourTree.Tree
{
    [SerializeField]
    private Unit _unit;

    protected override TreeNode SetupTree()
    {
        TreeNode root = new Sequence(this, new List<TreeNode>
        {
            new CheckCanDoActions(this, _unit),
            new Selector(this, new List<TreeNode>
            {
                new Sequence(this, new List<TreeNode>
                {
                    new Selector(this, new List<TreeNode>
                    {
                        new CheckPositionToBuild(this, _unit),
                        new CheckTargetToAttack(this, _unit),
                    }),
                    new TaskMove(this, _unit)
                }),
                new Sequence(this, new List<TreeNode>
                {
                    new CheckUnitInBuildPosition(this, _unit),
                    new TaskBuild(this, _unit)
                }),
                new Sequence(this, new List<TreeNode>
                {
                    new CheckEnemyInAttackRange(this, _unit),
                    new TaskAttack(this, _unit)
                }),
                new TaskFinalize(this, _unit)
            })
        });

        return root;
    }
}
