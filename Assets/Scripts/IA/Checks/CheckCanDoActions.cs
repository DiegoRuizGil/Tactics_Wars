using BehaviourTree;

public class CheckCanDoActions : TreeNode
{
    private readonly Unit _unit;

    public CheckCanDoActions(Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        if (GameManager.Instance.CurrentTeam != _unit.Team)
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }

        _state = _unit.HasFinished ? TreeNodeState.FAILURE : TreeNodeState.SUCCESS;
        return _state;
    }
}
