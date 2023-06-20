using UnityEngine;

public class Building : Entity
{
    [Header("Building Settings")]
    [SerializeField]
    private BuildingType _buildingType;

    private AnimationEventSystem _animEventSys;

    public BuildingType BuildingType { get { return _buildingType; } set { _buildingType = value; } }

    private void Start()
    {
        _animEventSys = GetComponent<AnimationEventSystem>();

        if (_entityDamagedEvent != null)
            _entityDamagedEvent?.Invoke(CurrentHealth * 1f / MaxHealth, false);
    }

    public override void EntityDeath()
    {
        GameManager.Instance.RemoveBuilding(this);

        if (_animEventSys != null)
            _animEventSys.FinishAnimation();
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
