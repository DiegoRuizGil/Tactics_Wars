using System.Collections.Generic;
using UnityEngine;

public class Node
{
    #region Variables
    private Vector3 _position;
    private int _gridX;
    private int _gridY;
    private int _distanceCost;
    private int _gCost;
    private int _hCost;
    private Node _nodeParent;
    private List<Node> _neighbours;
    private bool _isWall;
    private readonly Entity[] _nodeEntities;
    private ResourceType _resource;
    #endregion

    #region Getters Setters
    public Vector3 Position { get { return _position; } set { _position = value; } }
    public int GridX { get { return _gridX; } set { _gridX = Mathf.Max(0, value); } }
    public int GridY { get { return _gridY; } set { _gridY = Mathf.Max(0, value); } }
    public int DistanceCost { get { return _distanceCost; } set { _distanceCost = value; } }
    public int GCost { get { return _gCost; } set { _gCost = value; } }
    public int HCost { get { return _hCost; } set { _hCost = value; } }
    public int FCost { get { return _gCost + _hCost; } }
    public Node NodeParent { get { return _nodeParent; } set { _nodeParent = value; } }
    public List<Node> Neighbours { get { return _neighbours; } set { _neighbours = value; } }
    public bool IsWall { get { return _isWall; } set { _isWall = value; } }
    public ResourceType Resource { get { return _resource; } set { _resource = value; } }
    #endregion

    public Node(Vector3 position, int gridX, int gridY)
    {
        _position = position;
        _gridX = gridX;
        _gridY = gridY;

        _distanceCost = 0;
        _gCost = int.MaxValue / 2;
        _hCost = int.MaxValue / 2;
        
        _nodeParent = null;
        _neighbours = new List<Node>();
        _nodeEntities = new Entity[2];
    }

    public Node(Vector3 position, int gridX, int gridY, Unit unit, Building building)
        : this(position, gridX, gridY)
    {
        if (unit != null)
            _nodeEntities[1] = unit;
        if (building != null)
            _nodeEntities[0] = building;
    }

    public Entity GetTopEntity()
    {
        if (_nodeEntities[1] != null)
            return _nodeEntities[1];
        else if (_nodeEntities[0] != null)
            return _nodeEntities[0];
        else
            return null;
    }

    public Entity RemoveTopEntity()
    {
        Entity removedEntity = null;
        if (_nodeEntities[1] != null)
        {
            removedEntity = _nodeEntities[1];
            _nodeEntities[1] = null;
            return removedEntity;
        }  
        else if (_nodeEntities[0] != null)
        {
            removedEntity = _nodeEntities[0];
            _nodeEntities[0] = null;
            return removedEntity;
        }  
        else
            return removedEntity;
    }

    public bool AddEntity(Entity entity)
    {
        if (_nodeEntities[1] == null && entity as Unit)
        {
            _nodeEntities[1] = entity;
            return true;
        } 
        else if (_nodeEntities[0] == null)
        {
            _nodeEntities[0] = entity;
            return true;
        }   
        else
        {
            Debug.LogWarning($"Intento de añadir más de dos entidades al nodo ({GridX}, {GridY})");
            return false;
        } 
    }

    public Entity GetEntity(int index)
    {
        if (index > 1)
            return null;
        return _nodeEntities[index];
    }

    public bool CanBuildUnitBuilding(TeamEnum team)
    {
        foreach (Node neighhour in Neighbours)
        {
            if (neighhour.GetEntity(0) == null)
                continue;

            Building building = neighhour.GetEntity(0) as Building;

            if (building.Team != team)
                continue;

            if (building.BuildingType == BuildingType.URBAN_CENTER)
                return true;
        }

        return false;
    }

    public bool CanBuildFarm(TeamEnum team)
    {
        foreach (Node neighhour in Neighbours)
        {
            if (neighhour.GetEntity(0) == null)
                continue;

            Building building = neighhour.GetEntity(0) as Building;

            if (building.Team != team)
                continue;

            if (building.BuildingType == BuildingType.WINDMILL)
                return true;
        }

        return false;
    }

    public void ShowInfo()
    {
        Debug.Log($"({GridX}, {GridY}) Edificio: {_nodeEntities[0]?.Name}. Unidad: {_nodeEntities[1]?.Name}. Recurso: {_resource}.");
    }
}
