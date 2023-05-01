using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviourTree;

public class TaskBuild : TreeNode
{
    private Unit _unit;

    public TaskBuild(BehaviourTree.Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        if (_unit.TryGetComponent(out BuildingGenerator buildingGenerator))
        {
            List<BuildingInfoSO> buildings = buildingGenerator.GetBuildingsToBuild();

            if (buildings.Count <= 0)
            {
                _state = TreeNodeState.FAILURE;
                return _state;
            }
            else if (buildings.Count == 1) // resource building
            {
                if (GameManager.Instance.HasEnoughResources(buildings[0].FoodAmount, buildings[0].GoldAmount, _unit.Team))
                {
                    BaseAction buildAction = new BuildAction(buildings[0], _unit, _unit.Team);
                    buildAction.Execute();

                    Tree.ClearData("targetPosition");

                    _state = TreeNodeState.SUCCESS;
                    return _state;
                }
                else
                {
                    _state = TreeNodeState.FAILURE;
                    return _state;
                }
            }
            else // units buildings
            {
                List<string> builtBuildings = new List<string>();

                Building urbanCenter = GameManager.Instance.BuildingLists[_unit.Team]
                    .Where(building => building.BuildingType == BuildingType.URBAN_CENTER).FirstOrDefault();
                Node nodeUrbanCenter = Grid.Instance.GetNode(urbanCenter.transform.position);

                foreach (Node neighbour in nodeUrbanCenter.Neighbours)
                {
                    if (neighbour.GetEntity(0) != null)
                        builtBuildings.Add(neighbour.GetEntity(0).Name);
                }

                BuildingInfoSO selectedBuilding = SelectUnitBuilding(buildings, builtBuildings);

                if (selectedBuilding == null)
                {
                    _state = TreeNodeState.FAILURE;
                    return _state;
                }
                else
                {
                    BaseAction buildAction = new BuildAction(selectedBuilding, _unit, _unit.Team);
                    buildAction.Execute();

                    Tree.ClearData("targetPosition");

                    _state = TreeNodeState.SUCCESS;
                    return _state;
                }
            }
        }
        else
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }
    }

    private BuildingInfoSO SelectUnitBuilding(List<BuildingInfoSO> buildings, List<string> builtBuildings)
    {
        BuildingInfoSO selectedBuilding = null;

        if (builtBuildings.Count < 3)
        {
            foreach (BuildingInfoSO buildingInfo in buildings)
            {
                if (!builtBuildings.Contains(buildingInfo.Entity.Name) &&
                    GameManager.Instance.HasEnoughResources(buildingInfo.FoodAmount, buildingInfo.GoldAmount, _unit.Team))
                {
                    selectedBuilding = buildingInfo;
                    break;
                }
            }
        }
        else if (builtBuildings.Count == 3)
        {
            while (buildings.Count > 0 && selectedBuilding == null)
            {
                int index = Random.Range(0, buildings.Count);

                if (GameManager.Instance.HasEnoughResources(buildings[index].FoodAmount, buildings[index].GoldAmount, _unit.Team))
                {
                    selectedBuilding = buildings[index];
                }
                else
                {
                    buildings.RemoveAt(index);
                }
            }
        }

        return selectedBuilding;
    }
}
