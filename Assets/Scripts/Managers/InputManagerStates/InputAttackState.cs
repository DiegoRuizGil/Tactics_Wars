using UnityEngine;

public class InputAttackState : InputBaseState
{
    public override void EnterState(InputManager manager)
    {
        Debug.Log("Has entrado en el estado de la acci�n atacar");
    }

    public override void UpdateState(InputManager manager)
    {
        Debug.Log("Update desde el estado de la acci�n atacar");
        if (manager.ChangeAttackState)
            manager.SwitchState(manager.MoveState);
    }
}
