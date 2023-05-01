using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _maxVillagersAmount = 3;

    private readonly Dictionary<TeamEnum, List<ResourceType>> _buildMode = new Dictionary<TeamEnum, List<ResourceType>>();
    private readonly List<ResourceType> _resourceTypeList = new List<ResourceType>
    {
        ResourceType.FOOD,
        ResourceType.GOLD,
        ResourceType.NONE,
    };

    public void ManageEntities(TeamEnum team)
    {
        if (team != GameManager.Instance.PlayerTeam)
        {
            // manage villagers build mode
            if (!_buildMode.ContainsKey(team))
            {
                _buildMode[team] = new List<ResourceType>(_resourceTypeList);
            }
            else
            {
                List<ResourceType> currentBuildModes = new List<ResourceType>();
                foreach (Unit villager in GameManager.Instance.GetUnitsOfType(team, UnitType.ALDEANO))
                {
                    if (villager.TryGetComponent(out BehaviourTree.Tree tree))
                    {
                        if (tree.GetData("buildMode") != null)
                            currentBuildModes.Add((tree.GetData("buildMode") as ResourceType?).Value);
                    }
                }

                _buildMode[team].AddRange(_resourceTypeList.Except(currentBuildModes).ToList());
            }

            StartCoroutine(nameof(ManageEntitiesCoroutine), team);
        }
    }

    private IEnumerator ManageEntitiesCoroutine(TeamEnum team)
    {
        yield return StartCoroutine(nameof(ManageUnits), team);

        yield return StartCoroutine(nameof(ManageBuildings), team);

        GameManager.Instance.FinalizeCurrentTurn();
    }

    private IEnumerator ManageUnits(TeamEnum team)
    {
        List<Unit> units = GameManager.Instance.UnitLists[team];

        if (units.Count > 0)
        {
            int index = 0;
            BehaviourTree.Tree unitTree = null;
            Unit currentUnit;
            while (index < units.Count)
            {
                currentUnit = units[index];
                if (unitTree == null)
                {
                    if (currentUnit.TryGetComponent(out BehaviourTree.Tree tree))
                    {
                        // set villager build mode
                        if (currentUnit.UnitType == UnitType.ALDEANO)
                        {
                            if (tree.GetData("buildMode") == null && _buildMode[team].Count > 0)
                            {
                                tree.SetData("buildMode", _buildMode[team][0]);
                                _buildMode[team].RemoveAt(0);
                            }
                        }

                        unitTree = tree;
                        unitTree.enabled = true;
                    }
                    else
                    {
                        index++;
                    }
                }
                else
                {
                    if (currentUnit.HasFinished)
                    {
                        unitTree.enabled = false;
                        unitTree = null;
                        index++;
                    }
                }

                yield return null;
            }
        }

        yield return null;
    }

    private IEnumerator ManageBuildings(TeamEnum team)
    {
        int unitAmount = GameManager.Instance.UnitLists[team].Count;

        if (unitAmount >= GameManager.Instance.MaxUnitAmount)
            yield break;

        ManageUrbanCenter(team);

        ManageUnitBuildings(team);

        yield return null;
    }

    private void ManageUrbanCenter(TeamEnum team)
    {
        int unitAmount = GameManager.Instance.UnitLists[team].Count;

        Building urbanCenter = GameManager.Instance.BuildingLists[team]
            .Where(building => building.BuildingType == BuildingType.URBAN_CENTER
                    && Grid.Instance.GetNode(building.transform.position).GetEntity(1) == null)
            .FirstOrDefault();

        int villagersAmount = GameManager.Instance.UnitLists[team]
            .Where(unit => unit.UnitType == UnitType.ALDEANO)
            .ToList().Count;

        // create new villager
        if (villagersAmount < _maxVillagersAmount && urbanCenter != null && unitAmount + 1 <= GameManager.Instance.MaxUnitAmount)
        {
            if (urbanCenter.TryGetComponent(out UnitGenerator unitGenerator))
            {
                UnitInfoSO villagerInfo = unitGenerator.UnitsInfo[0];

                if (GameManager.Instance.UpdateResources(team, villagerInfo.FoodAmount, villagerInfo.GoldAmount))
                {
                    GameManager.Instance.InstantiateUnit(
                        villagerInfo.Entity,
                        urbanCenter.transform.position,
                        team
                    );
                }
            }
        }
    }

    private void ManageUnitBuildings(TeamEnum team)
    {
        int unitAmount = GameManager.Instance.UnitLists[team].Count;

        List<Building> unitBuildings = GameManager.Instance.BuildingLists[team]
            .Where(building => building.BuildingType == BuildingType.UNIT_BUILDING
                    && Grid.Instance.GetNode(building.transform.position).GetEntity(1) == null)
            .ToList();

        foreach (Building building in unitBuildings)
        {
            if (building.TryGetComponent(out UnitGenerator unitGenerator))
            {
                List<UnitInfoSO>  unitInfoList = new List<UnitInfoSO>(unitGenerator.UnitsInfo);
                if (unitInfoList.Count <= 0)
                    break;

                int randomIndex = Random.Range(0, unitInfoList.Count);
                UnitInfoSO unitInfo = unitInfoList[randomIndex];

                while (!GameManager.Instance.UpdateResources(team, unitInfo.FoodAmount, unitInfo.GoldAmount))
                {
                    unitInfoList.RemoveAt(randomIndex);
                    randomIndex = Random.Range(0, unitInfoList.Count);
                    unitInfo = unitInfoList[randomIndex];
                }

                if (unitAmount + 1 <= GameManager.Instance.MaxUnitAmount)
                {
                    GameManager.Instance.InstantiateUnit(
                        unitInfo.Entity,
                        building.transform.position,
                        team
                    );
                    unitAmount++;
                }
            }

            if (unitAmount >= GameManager.Instance.MaxUnitAmount)
                break;
        }
    }
}
