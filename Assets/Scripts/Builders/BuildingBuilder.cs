using UnityEngine;

public class BuildingBuilder : IBuilder<Building>
{
    private readonly Building _building;

    public BuildingBuilder()
    {
        GameObject go = new GameObject();
        _building = go.AddComponent<Building>();
    }

    public BuildingBuilder WithPosition(Vector3 position)
    {
        _building.transform.position = position;
        return this;
    }

    public Building Build()
    {
        return _building;
    }

    public static implicit operator Building(BuildingBuilder builder)
    {
        return builder.Build();
    }
}
