using UnityEngine;

public class InputRepairState : InputBaseState
{
    public InputRepairState(InputManager context, InputStateFactory factory)
        : base(context, factory)
    {
        NextState = factory.NoAction();
    }

    public override void EnterState()
    {
        Debug.Log("<color=cyan>Repair</color>: Entering the state");
        BaseAction action = new RepairAction(Context.SelectedUnit);
        Context.ActionHandler.ActionToHandle = action;
        Context.ActionHandler.ExecuteCommand();
    }

    public override void UpdateState()
    {
        SwitchToNextState();
    }
}
