using System;
using System.Collections.Generic;
using UnityEngine;

public class InputAttackState : InputBaseState
{
    private List<Vector3> _attackArea;

    public InputAttackState(InputManager context, InputStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        //Debug.Log("<color=red>Attack</color>: Entering the state");

        _attackArea = Pathfinding.Instance.GetAttackArea(
            Context.SelectedUnit.transform.position,
            Context.SelectedUnit.AttackRange);

        if (Context.UpdateActionAreaEvent != null)
            Context.UpdateActionAreaEvent.Raise(_attackArea);
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            try
            {
                Vector3 mousePosition = Context.GetMouseNodePosition();
                if (_attackArea.Contains(mousePosition))
                {
                    Entity entityToAttack = Grid.Instance.GetNode(mousePosition).GetTopEntity();

                    BaseAction action = new AttackAction(Context.SelectedUnit, entityToAttack);
                    Context.ActionHandler.ActionToHandle = action;
                    Context.ActionHandler.ExecuteCommand();

                    NextState = Factory.Waiting(action);
                    SwitchToNextState();
                }
                else
                {
                    SwitchState(Factory.NoAction());
                }
            }
            catch (NullReferenceException ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
