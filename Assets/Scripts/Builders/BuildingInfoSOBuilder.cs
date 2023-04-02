using UnityEngine;

public class BuildingInfoSOBuilder : IBuilder<BuildingInfoSO>
{
    private readonly BuildingInfoSO _info;

    public BuildingInfoSOBuilder()
    {
        _info = ScriptableObject.CreateInstance<BuildingInfoSO>();
    }

    public BuildingInfoSOBuilder WithBuilding(Building building)
    {
        _info.Entity = building;
        return this;
    }

    public BuildingInfoSOBuilder WithFoodAmount(int amount)
    {
        _info.FoodAmount = amount;
        return this;
    }

    public BuildingInfoSOBuilder WithGoldAmount(int amount)
    {
        _info.GoldAmount = amount;
        return this;
    }

    public BuildingInfoSO Build()
    {
        return _info;
    }

    public static implicit operator BuildingInfoSO(BuildingInfoSOBuilder builder)
    {
        return builder.Build();
    }
}
