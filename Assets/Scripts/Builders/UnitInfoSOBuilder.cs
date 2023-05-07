using UnityEngine;

public class UnitInfoSOBuilder : IBuilder<UnitInfoSO>
{
    private readonly UnitInfoSO _info;

    public UnitInfoSOBuilder()
    {
        _info = ScriptableObject.CreateInstance<UnitInfoSO>();
    }

    public UnitInfoSOBuilder WithUnit(Unit unit)
    {
        _info.Entity = unit;
        return this;
    }

    public UnitInfoSOBuilder WithFoodAmount(int amount)
    {
        _info.FoodAmount = amount;
        return this;
    }

    public UnitInfoSOBuilder WithGoldAmount(int amount)
    {
        _info.GoldAmount = amount;
        return this;
    }

    public UnitInfoSO Build()
    {
        return _info;
    }

    public static implicit operator UnitInfoSO(UnitInfoSOBuilder builder)
    {
        return builder.Build();
    }
}
