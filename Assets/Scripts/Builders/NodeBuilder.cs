using UnityEngine;

public class NodeBuilder : IBuilder<Node>
{
    private int _gridX;
    private int _gridY;
    private Vector3 _position;
    private Unit _unit;
    private Building _building;

    public NodeBuilder WithGridPosition(int x, int y)
    {
        _gridX = x;
        _gridY = y;
        return this;
    }

    public NodeBuilder WithWorldPosition(Vector3 position)
    {
        _position = position;
        return this;
    }

    public NodeBuilder WithUnit(Unit unit)
    {
        _unit = unit;
        return this;
    }

    public NodeBuilder WithBuilding(Building building)
    {
        _building = building;
        return this;
    }

    public Node Build()
    {
        return new Node(_position, _gridX, _gridY, _unit, _building);
    }

    public static implicit operator Node(NodeBuilder builder)
    {
        return builder.Build();
    }
}
