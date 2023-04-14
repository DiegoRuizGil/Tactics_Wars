using UnityEngine;

public class InputGenerateUnitState : InputBaseState
{
    private readonly UnitInfoSO _unitInfo;

    public InputGenerateUnitState(InputManager context, InputStateFactory factory, UnitInfoSO unitInfo)
        : base(context, factory)
    {
        _unitInfo = unitInfo;
        NextState = factory.NoAction();
    }

    public override void EnterState()
    {
        Debug.Log("<color=yellow>Generate Unit</color>: Entering the state");
        BaseAction action = new GenerateUnitAction(
            _unitInfo,
            Context.SelectedBuilding.transform.position,
            TeamEnum.BLUE
        );
        Context.ActionHandler.ActionToHandle = action;
        Context.ActionHandler.ExecuteCommand();
    }

    public override void UpdateState()
    {
        SwitchToNextState();
    }
}
