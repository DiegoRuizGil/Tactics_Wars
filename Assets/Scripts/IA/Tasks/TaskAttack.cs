using BehaviourTree;

public class TaskAttack : TreeNode
{
    private Unit _attacker;
    private BaseAction _action;

    public TaskAttack(Tree tree, Unit attacker)
        : base(tree)
    {
        _attacker = attacker;
    }

    public override TreeNodeState Evaluate()
    {
        Entity target = Tree.GetData("target") as Entity;

        if (_action == null)
        {
            _action = new AttackAction(_attacker, target);
            _action.Execute();

            if (target.CurrentHealth <= 0)
                Tree.ClearData("target");

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
