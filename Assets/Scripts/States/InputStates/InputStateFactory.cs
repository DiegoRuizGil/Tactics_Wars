using UnityEngine;

public class InputStateFactory
{
    private readonly InputManager _context;

    public InputStateFactory(InputManager context)
    {
        _context = context;
    }

    public InputBaseState NoAction()
    {
        return new InputNoActionState(_context, this);
    }

    public InputBaseState MoveAction()
    {
        return new InputMoveState(_context, this);
    }

    public InputBaseState AttackAction()
    {
        return new InputAttackState(_context, this);
    }

    public InputBaseState BuildAction(EntityInfoSO buildingInfo)
    {
        return new InputBuildState(_context, this, buildingInfo);
    }

    public InputBaseState RepairAction()
    {
        return new InputRepairState(_context, this);
    }

    public InputBaseState FinalizeUnit()
    {
        return new InputFinalizeUnitState(_context, this);
    }

    public InputBaseState Waiting(BaseAction action)
    {
        return new InputWaitingState(_context, this, action);
    }
}
