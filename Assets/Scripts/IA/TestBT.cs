using UnityEngine;
using BehaviourTree;

[RequireComponent(typeof(Unit))]
public class TestBT : BehaviourTree.Tree
{
    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    protected override TreeNode SetupTree()
    {
        TreeNode root = new TaskFinalize(this, _unit);

        return root;
    }
}
