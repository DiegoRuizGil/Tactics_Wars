using System.Linq;
using UnityEngine;

public class UnitBuilder : IBuilder<Unit>
{
    private readonly Unit _unit;

    public UnitBuilder()
    {
        GameObject go = new GameObject();
        _unit = go.AddComponent<Unit>();
    }

    public UnitBuilder WithTeam(TeamEnum team)
    {
        _unit.Team = team;
        return this;
    }

    public UnitBuilder WithUnitType(UnitType unitType)
    {
        _unit.UnitType = unitType;
        return this;
    }

    public UnitBuilder WithWeaknesess(params UnitType[] weaknesess)
    {
        _unit.Weaknesses = weaknesess.ToList();
        return this;
    }

    public UnitBuilder WithPosition(Vector3 position)
    {
        _unit.transform.position = position;
        return this;
    }

    public UnitBuilder WithName(string name)
    {
        _unit.Name = name;
        return this;
    }

    public Unit Build()
    {
        return _unit;
    }

    public static implicit operator Unit(UnitBuilder builder)
    {
        return builder.Build();
    }
}
