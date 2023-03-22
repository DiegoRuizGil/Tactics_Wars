using UnityEngine;

public class UnitBuildingBuilder : IBuilder<UnitBuilding>
{
    private readonly UnitBuilding _building;

    public UnitBuildingBuilder()
    {
        GameObject go = new GameObject();
        _building = go.AddComponent<UnitBuilding>();
    }

    public UnitBuildingBuilder WithPosition(Vector3 position)
    {
        _building.transform.position = position;
        return this;
    }

    public UnitBuilding Build()
    {
        return _building;
    }

    public static implicit operator UnitBuilding(UnitBuildingBuilder builder)
    {
        return builder.Build();
    }
}
