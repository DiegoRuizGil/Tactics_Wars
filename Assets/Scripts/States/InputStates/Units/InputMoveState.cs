using System;
using System.Collections.Generic;
using UnityEngine;

public class InputMoveState : InputBaseState
{
    private List<Vector3> _moveAreaPositions;
    private List<Vector3> _arrowPathPositions;

    public InputMoveState(InputManager context, InputStateFactory factory)
        : base(context, factory) { }

    public override void EnterState()
    {
        Debug.Log("<color=yellow>Move</color>: Entering the state");

        if (Context.UpdateActionAreaEvent == null)
            return;
        if (Context.SelectedUnit == null)
            return;
        
        Vector3 unitPosition = Context.SelectedUnit.transform.position;
        int moveCost = 1;
        int maxDistance = Context.SelectedUnit.MovementRange;

        _moveAreaPositions = Pathfinding.Instance.GetMovementArea(unitPosition, moveCost, maxDistance);
        Context.UpdateActionAreaEvent.Raise(_moveAreaPositions);
    }

    public override void UpdateState()
    {
        UpdateArrowPath();

        if (Input.GetMouseButtonDown(0))
        {
            try
            {
                Vector3 mousePosition = Context.GetMouseWorldPosition();
                Node node = Grid.Instance.GetNode(mousePosition);
                if (node != null)
                {
                    if (!_moveAreaPositions.Contains(node.Position))
                    {
                        SwitchState(Factory.NoAction());
                    }
                    else
                    {
                        BaseAction action = new MoveAction(Context.SelectedUnit, _arrowPathPositions, Context.MovementSpeed);
                        NextState = Factory.Waiting(action);
                        Context.ActionHandler.ActionToHandle = action;
                        Context.ActionHandler.ExecuteCommand();
                        SwitchToNextState();
                    }
                }
                else
                {
                    SwitchState(Factory.NoAction());
                }
            }
            catch (GridOutOfBoundsException ex)
            {
                Debug.LogWarning($"{ex.GetType()}: Ratón fuera del grid");
            }
        }
    }

    private void UpdateArrowPath()
    {
        if (Context.UpdateMovementArrowEvent != null)
        {
            try
            {
                Vector3 mousePosition = Context.GetMouseNodePosition();
                if (mousePosition != Vector3.zero && _moveAreaPositions.Contains(mousePosition))
                {
                    // needs to perform GetMovementArea (EnterState)
                    _arrowPathPositions = Pathfinding.Instance.CalculatePositionsPath(mousePosition);
                    Context.UpdateMovementArrowEvent.Raise(_arrowPathPositions);
                }
            }
            catch (GridOutOfBoundsException ex)
            {
                Debug.LogWarning($"{ex.GetType()}: Ratón fuera del grid");
            }
        }
    }
}
