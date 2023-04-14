using BehaviourTree;

public class CheckTargetToAttack : TreeNode
{
    private Unit _unit;

    public CheckTargetToAttack(Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        if (_unit.HasMoved)
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }

        Entity target = Tree.GetData("target") as Entity;
        if (target == null)
        {
            // SELECCIONAR ENTIDAD ENEMIGA
            Entity newTarget = GameManager.Instance.UnitLists[TeamEnum.BLUE][0];
            if (newTarget == null)
            {
                newTarget = GameManager.Instance.BuildingLists[TeamEnum.BLUE][0];
                if (newTarget == null)
                {
                    _state = TreeNodeState.FAILURE;
                    return _state;
                }
            }

            Tree.SetData("target", newTarget);
            Tree.SetData("targetPosition", newTarget.transform.position);

            _state = TreeNodeState.SUCCESS;
            return _state;
        }
        else
        {
            _state = TreeNodeState.SUCCESS;
            return _state;
        }
    }
}
