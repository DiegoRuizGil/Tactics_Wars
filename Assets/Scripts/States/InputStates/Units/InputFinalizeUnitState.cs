using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFinalizeUnitState : InputBaseState
{
    public InputFinalizeUnitState(InputManager context, InputStateFactory factory)
        : base(context, factory)
    {
        NextState = Factory.NoAction();
    }

    public override void EnterState()
    {
        //Debug.Log("<color=blue>Finalize Unit</color>: Entering the state");
        Context.SelectedUnit.HasFinished = true;
        SwitchToNextState();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
