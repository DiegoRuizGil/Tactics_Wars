using System.Collections.Generic;
using UnityEngine;

public class InputWaitingState : InputBaseState
{
    private Unit _unit;

    public InputWaitingState(InputManager context, InputStateFactory factory, BaseAction action)
        : base(context, factory)
    {
        NextState = Factory.NoAction();
        _unit = Context.SelectedUnit;

        action.ActionFinished += SwitchToNextState;
        action.ActionFinished += FinalizeUnit;
    }

    public override void EnterState()
    {
        //Debug.Log("<color=cyan>Waiting</color>: Entering the state");
        Context.SelectedUnit = null;
        Context.OnEntityDeselectedEvent.Raise();
    }

    public override void UpdateState()
    {
        //Debug.Log("<color=cyan>Waiting</color> to finish the action");
    }

    private void FinalizeUnit() // check if unit can do more actions
    {
        if (!CheckAttackAction() && !CheckBuildAction() && !CheckRepairAction())
        {
            _unit.HasFinished = true;
        }
    }

    private bool CheckAttackAction()
    {
        List<Vector3> attackArea = Pathfinding.Instance.GetAttackArea(
            _unit.transform.position,
            _unit.AttackRange);

        return attackArea.Count > 0;
    }

    private bool CheckBuildAction()
    {
        if (_unit.gameObject.GetComponent<BuildingGenerator>() == null)
            return false;

        Node node = Grid.Instance.GetNode(_unit.transform.position);

        if (node.CanBuildUnitBuilding(_unit.Team))
            return true;
        else if (node.CanBuildFarm(_unit.Team))
            return true;
        else if (node.Resource != ResourceType.NONE && node.GetEntity(0) == null)
            return true;
        else
            return false;
    }

    private bool CheckRepairAction()
    {
        if (_unit.UnitType != UnitType.ALDEANO)
        {
            return false;
        }

        Node node = Grid.Instance.GetNode(_unit.transform.position);
        Entity building = node.GetEntity(0);
        if (building != null)
        {
            return building.MaxHealth != building.CurrentHealth;
        }
        else
        {
            return false;
        }
    }
}
