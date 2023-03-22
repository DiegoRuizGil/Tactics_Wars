using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfoSOBuilder : IBuilder<EntityInfoSO>
{
    private readonly EntityInfoSO _info;

    public EntityInfoSOBuilder()
    {
        _info = ScriptableObject.CreateInstance<EntityInfoSO>();
    }

    public EntityInfoSOBuilder WithPrefab(GameObject prefab)
    {
        _info.EntityPrefab = prefab;
        return this;
    }

    public EntityInfoSOBuilder WithFoodAmount(int amount)
    {
        _info.FoodAmount = amount;
        return this;
    }

    public EntityInfoSOBuilder WithGoldAmount(int amount)
    {
        _info.GoldAmount = amount;
        return this;
    }

    public EntityInfoSO Build()
    {
        return _info;
    }

    public static implicit operator EntityInfoSO(EntityInfoSOBuilder builder)
    {
        return builder.Build();
    }
}
