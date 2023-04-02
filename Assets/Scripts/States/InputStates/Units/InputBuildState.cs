using UnityEngine;

public class InputBuildState : InputBaseState
{
    private BuildingInfoSO _buildingInfo;

    public InputBuildState(InputManager context, InputStateFactory factory, BuildingInfoSO buildingInfo)
        : base(context, factory)
    {
        _buildingInfo = buildingInfo;
        NextState = factory.NoAction();
    }

    public override void EnterState()
    {
        Debug.Log("<color=magenta>Build</color>: Entering the state");
        BaseAction action = new BuildAction(_buildingInfo, Context.SelectedUnit, Context.BuildingsParentBlueTeam);
        Context.ActionHandler.ActionToHandle = action;
        Context.ActionHandler.ExecuteCommand();
    }

    public override void UpdateState()
    {
        SwitchToNextState();
    }
}
