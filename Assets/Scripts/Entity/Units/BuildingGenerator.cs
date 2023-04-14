using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField]
    private BuildingInfoSO[] _buildingsInfo;

    public BuildingInfoSO[] BuildingsInfo { get { return _buildingsInfo; } }

    public List<BuildingInfoSO> GetBuildingsToBuild()
    {
        List<BuildingInfoSO> buildings = new List<BuildingInfoSO>();
        TeamEnum team = GetComponent<Unit>().Team;

        Node currentNode = Grid.Instance.GetNode(transform.position);
        foreach (BuildingInfoSO buildingInfo in _buildingsInfo)
        {
            switch (buildingInfo.Entity.BuildingType)
            {
                case BuildingType.UNIT_BUILDING:
                    if (currentNode.CanBuildUnitBuilding(team))
                        buildings.Add(buildingInfo);
                    break;
                
                case BuildingType.WINDMILL:
                    if (currentNode.Resource == ResourceType.FOOD)
                        buildings.Add(buildingInfo);
                    break;
                
                case BuildingType.FARM:
                    if (currentNode.CanBuildFarm(team))
                        buildings.Add(buildingInfo);
                    break;
                
                case BuildingType.GOLDMINE:
                    if (currentNode.Resource == ResourceType.GOLD)
                        buildings.Add(buildingInfo);
                    break;
                
                default:
                    break;
            }
        }

        return buildings;
    }
}
