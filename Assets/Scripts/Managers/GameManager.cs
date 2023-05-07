using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _maxUnitAmount = 10;
    [SerializeField]
    private int _initialFood = 450;
    [SerializeField]
    private int _initialGold = 450;

    [Header("Blue Team Entity Parents")]
    [SerializeField]
    private Transform _blueTeamUnitParent;
    [SerializeField]
    private Transform _blueTeamBuildingParent;

    [Header("Red Team Entity Parents")]
    [SerializeField]
    private Transform _redTeamUnitParent;
    [SerializeField]
    private Transform _redTeamBuildingParent;

    [Header("Game Events")]
    [SerializeField]
    private TeamEnumEvent _onTurnUpdate;
    [SerializeField]
    private VoidEvent _onTopHUDUpdate;

    [Header("Debug")]
    [SerializeField]
    private int _debugFoodAmount;
    [SerializeField]
    private int _debugGoldAmount;

    private TeamEnum _playerTeam = TeamEnum.BLUE;

    private Dictionary<TeamEnum, int> _foodResources;
    private Dictionary<TeamEnum, int> _goldResources;

    private Dictionary<TeamEnum, Transform> _unitParents;
    private Dictionary<TeamEnum, Transform> _buildingParents;

    private Dictionary<TeamEnum, List<Unit>> _unitLists;
    private Dictionary<TeamEnum, List<Building>> _buildingLists;

    [Space(20)]
    [SerializeField]
    private int _turn;
    [SerializeField]
    private TeamEnum _currentTeam;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public int MaxUnitAmount { get { return _maxUnitAmount; } }
    public TeamEnum PlayerTeam { get { return _playerTeam; } }
    public Dictionary<TeamEnum, int> FoodResources { get { return _foodResources; } }
    public Dictionary<TeamEnum, int> GoldResources { get { return _goldResources; } }
    public Dictionary<TeamEnum, List<Unit>> UnitLists { get { return _unitLists; } }
    public Dictionary<TeamEnum, List<Building>> BuildingLists { get { return _buildingLists; } }
    public TeamEnum CurrentTeam { get { return _currentTeam; } }

    private void Awake()
    {
        _instance = this;

        // SET RESOURCES
        _foodResources = new Dictionary<TeamEnum, int>
        {
            {TeamEnum.BLUE, _initialFood},
            {TeamEnum.RED, _initialFood}
        };

        _goldResources = new Dictionary<TeamEnum, int>
        {
            {TeamEnum.BLUE, _initialGold},
            {TeamEnum.RED, _initialGold}
        };

        // SET ENTITIES LISTS
        _unitLists = new Dictionary<TeamEnum, List<Unit>>
        {
            {TeamEnum.BLUE,  new List<Unit>()},
            {TeamEnum.RED,  new List<Unit>()}
        };

        _buildingLists = new Dictionary<TeamEnum, List<Building>>
        {
            {TeamEnum.BLUE,  new List<Building>()},
            {TeamEnum.RED,  new List<Building>()}
        };

        // SET ENTITIES PARENTS
        _unitParents = new Dictionary<TeamEnum, Transform>
        {
            {TeamEnum.BLUE, _blueTeamUnitParent},
            {TeamEnum.RED, _redTeamUnitParent}
        };

        _buildingParents = new Dictionary<TeamEnum, Transform>
        {
            {TeamEnum.BLUE, _blueTeamBuildingParent},
            {TeamEnum.RED, _redTeamBuildingParent}
        };
    }

    private void Start()
    {
        if (_onTurnUpdate != null)
            _onTurnUpdate.Raise(_currentTeam);
    }

    public void FinalizeCurrentTurn()
    {
        // reset units actions
        foreach (Unit unit in _unitLists[_currentTeam])
        {
            unit.HasMoved = false;
            unit.HasFinished = false;
        }

        _turn++;
        _currentTeam = (_currentTeam == TeamEnum.BLUE) ? TeamEnum.RED : TeamEnum.BLUE;

        if (_buildingLists[_currentTeam].Count <= 0
            && _unitLists[_currentTeam].Count <= 0)
        {
            // lanzar evento de fin de partida
        }
        else
        {
            // recover units hp on top of buildings
            foreach (Unit unit in _unitLists[_currentTeam])
            {
                if (Grid.Instance.GetNode(unit.transform.position).GetEntity(0) != null)
                    unit.RecoverHealth(unit.MaxHealth / 4);
            }

            if (_onTurnUpdate != null)
                _onTurnUpdate.Raise(_currentTeam);
        }
    }

    [ContextMenu("Debug Resources")]
    private void UpdateDebugResources()
    {
        _foodResources[PlayerTeam] = _debugFoodAmount;
        _goldResources[PlayerTeam] = _debugGoldAmount;

        if (_onTopHUDUpdate != null)
            _onTopHUDUpdate.Raise();
    }

    [ContextMenu("Resources Red Team")]
    private void ShowResourcesRed()
    {
        Debug.Log($"[RED] Food: {_foodResources[TeamEnum.RED]} Gold: {_goldResources[TeamEnum.RED]}");
    }

    #region RESOURCES
    public bool HasEnoughResources(int foodAmount, int goldAmount, TeamEnum team)
    {
        return foodAmount <= _foodResources[team] && goldAmount <= _goldResources[team];
    }

    public bool UpdateResources(TeamEnum team, int foodAmount, int goldAmount)
    {
        if (_foodResources[team] + foodAmount < 0 || _goldResources[team] + goldAmount < 0)
        {
            return false;
        }
        else
        {
            UpdateFoodResources(team, foodAmount);
            UpdateGoldResources(team, goldAmount);

            if (_onTopHUDUpdate != null)
                _onTopHUDUpdate.Raise();

            return true;
        }
    }

    public bool UpdateResource(TeamEnum team, ResourceType resourceType, int amount)
    {
        bool hasUpdated = false;
        switch (resourceType)
        {
            case ResourceType.FOOD:
                hasUpdated = UpdateFoodResources(team, amount);
                break;
            case ResourceType.GOLD:
                hasUpdated = UpdateGoldResources(team, amount);
                break;
            default:
                Debug.LogWarning($"{resourceType}: Tipo de recurso no contemplado");
                break;
        }

        if (_onTopHUDUpdate != null)
            _onTopHUDUpdate.Raise();

        return hasUpdated;
    }

    private bool UpdateFoodResources(TeamEnum team, int amount)
    {
        if (_foodResources[team] + amount < 0)
        {
            Debug.LogWarning("[FOOD RESOURCE] No se puede actualizar el rescuros a un número negativo. No se realizará la actualización.");
            return false;
        }
        else
        {
            _foodResources[team] += amount;
            return true;
        } 
    }

    private bool UpdateGoldResources(TeamEnum team, int amount)
    {
        if (_goldResources[team] + amount < 0)
        {
            Debug.LogWarning("[GOLD RESOURCE] No se puede actualizar el rescuros a un número negativo. No se realizará la actualización.");
            return false;
        }
        else
        {
            _goldResources[team] += amount;
            return true;
        }  
    }
    #endregion

    #region ENTITIES
    public List<Unit> GetUnitsOfType(TeamEnum team, UnitType type)
    {
        return _unitLists[team].Where(unit => unit.UnitType == type).ToList();
    }

    public Entity InstantiateEntity(Entity entityPrefab, Vector3 pos, TeamEnum team)
    {
        return InstantiateEntity(entityPrefab, pos, Quaternion.identity, team);
    }

    public Entity InstantiateEntity(Entity entityPrefab, Vector3 pos, Quaternion rotation, TeamEnum team)
    {
        if (entityPrefab is Unit)
        {
            return InstantiateUnit(entityPrefab as Unit, pos, rotation, team);
        }
        else if (entityPrefab is Building)
        {
            return InstantiateBuilding(entityPrefab as Building, pos, rotation, team);
        }
        else
        {
            Debug.Log($"No se pudo crear instancia de la entidad {entityPrefab.name}, no es Unit ni Building.");
            return null;
        }
    }

    public Unit InstantiateUnit(Unit unitPrefab, Vector3 pos, TeamEnum team)
    {
        return InstantiateUnit(unitPrefab, pos, Quaternion.identity, team);
    }

    public Unit InstantiateUnit(Unit unitPrefab, Vector3 pos, Quaternion rotation, TeamEnum team)
    {
        Unit unit = GameObject.Instantiate(
                unitPrefab,
                pos,
                rotation,
                _unitParents[team]
            );

        _unitLists[team].Add(unit);
        unit.SetEntityInGrid();

        unit.Team = team;
        SpriteRenderer spriteRenderer = unit.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Material material = spriteRenderer.material;
            if (material != null)
                material.SetFloat("_IsRedTeam", team == TeamEnum.BLUE ? 0f : 1f);
        }

        return unit;
    }

    public Building InstantiateBuilding(Building buildingPrefab, Vector3 pos, TeamEnum team)
    {
        return InstantiateBuilding(buildingPrefab, pos, Quaternion.identity, team);
    }

    public Building InstantiateBuilding(Building buildingPrefab, Vector3 pos, Quaternion rotation, TeamEnum team)
    {
        Building building = GameObject.Instantiate(
                buildingPrefab,
                pos,
                rotation,
                _buildingParents[team]
            );

        _buildingLists[team].Add(building);
        building.SetEntityInGrid();

        building.Team = team;
        SpriteRenderer spriteRenderer = building.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Material material = spriteRenderer.material;
            if (material != null)
                material.SetFloat("_IsRedTeam", team == TeamEnum.BLUE ? 0f : 1f);
        }

        return building;
    }
    
    public void RemoveUnit(Unit unit)
    {
        _unitLists[unit.Team].Remove(unit);

        Node node = Grid.Instance.GetNode(unit.transform.position);
        node.RemoveTopEntity();

        Destroy(unit.gameObject);
    }

    public void RemoveBuilding(Building building)
    {
        _buildingLists[building.Team].Remove(building);

        Node node = Grid.Instance.GetNode(building.transform.position);
        node.RemoveTopEntity();

        Destroy(building.gameObject);
    }
    #endregion

    #region PARENTS
    public void SetUnitParent(TeamEnum team, Transform parent)
    {
        _unitParents[team] = parent;
    }

    public void SetBuildingParent(TeamEnum team, Transform parent)
    {
        _buildingParents[team] = parent;
    }
    #endregion

}
