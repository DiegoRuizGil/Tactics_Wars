using System.Collections.Generic;
using BehaviourTree;

public class CheckTargetToAttack : TreeNode
{
    private readonly Unit _unit;

    private const int WEAKNESS_COST = 4;
    private const int HP_COST = 1;

    private const int HP_INTERVALS = 5; // 20%

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
            Entity newTarget = SelectTarget();

            if (newTarget == null)
            {
                _state = TreeNodeState.FAILURE;
                return _state;
            }

            Tree.SetData("target", newTarget);
            Tree.SetData("targetPosition", newTarget.transform.position);

            _state = TreeNodeState.SUCCESS;
            return _state;
        }
        else
        {
            Tree.SetData("targetPosition", target.transform.position);
            _state = TreeNodeState.SUCCESS;
            return _state;
        }
    }

    private Entity SelectTarget()
    {
        Entity target = null;
        int minCost = int.MaxValue;
        int currentCost = 0;

        List<Unit> unitList = GameManager.Instance.UnitLists[GameManager.Instance.PlayerTeam];
        foreach (Unit enemyUnit in  unitList)
        {
            // APPLY DISTANCE COST
            currentCost += AStarPathfinding.Instance.GetPath(
                    _unit.transform.position,
                    enemyUnit.transform.position,
                    _unit.Team
                ).Count;

            // APPLY WEAKNESS COST
            if (_unit.Weaknesses.Contains(enemyUnit.UnitType))
            {
                // enemy strong against IA unit
                currentCost += WEAKNESS_COST;
            }
            else if (enemyUnit.Weaknesses.Contains(_unit.UnitType))
            {
                // enemy weak against IA unit
                currentCost -= WEAKNESS_COST;
            }

            // APPLY HP COST
            currentCost += HP_COST * (enemyUnit.CurrentHealth * HP_INTERVALS / enemyUnit.MaxHealth);

            if (currentCost < minCost)
            {
                minCost = currentCost;
                target = enemyUnit;
            }

            currentCost = 0;
        }

        List<Building> buildingList = GameManager.Instance.BuildingLists[GameManager.Instance.PlayerTeam];
        foreach (Building building in buildingList)
        {
            // APPLY DISTANCE COST
            currentCost += AStarPathfinding.Instance.GetPath(
                    _unit.transform.position,
                    building.transform.position,
                    _unit.Team
                ).Count;

            // APPLY HP COST
            currentCost += HP_COST * (building.CurrentHealth * HP_INTERVALS / building.MaxHealth);

            if (currentCost < minCost)
            {
                minCost = currentCost;
                target = building;
            }

            currentCost = 0;
        }

        return target;
    }
}
