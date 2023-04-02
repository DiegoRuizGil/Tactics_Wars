using UnityEngine;

public class Building : Entity
{
    [Header("Building Settings")]
    [SerializeField]
    private BuildingType _buildingType;

    public BuildingType BuildingType { get { return _buildingType; } set { _buildingType = value; } }

    public override void EntityDeath()
    {
        Destroy(gameObject);
    }
}

public enum BuildingType
{
    URBAN_CENTER,
    UNIT_BUILDING,
    WINDMILL,
    FARM,
    GOLDMINE
}
