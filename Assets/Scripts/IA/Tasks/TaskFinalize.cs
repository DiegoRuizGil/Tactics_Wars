using BehaviourTree;

public class TaskFinalize : TreeNode
{
    private readonly Unit _unit;

    public TaskFinalize(Tree tree, Unit unit)
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
