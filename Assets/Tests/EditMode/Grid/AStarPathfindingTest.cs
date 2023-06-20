using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AStarPathfindingTest
{
    private TeamEnum _team;

    [SetUp]
    public void SetUp()
    {
        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);

        Grid.Instance.GetNode(new Vector3(0, 1, 0)).IsWall = true;
        Grid.Instance.GetNode(new Vector3(1, 1, 0)).IsWall = true;
        Grid.Instance.GetNode(new Vector3(2, 1, 0)).IsWall = true;

        _team = TeamEnum.BLUE;
        Unit unit = An.Unit.WithTeam(_team).WithPosition(new Vector3(1, 0, 0));
        unit.SetEntityInGrid();

        Grid.Instance.SetNodesNeighbours();
    }

    [Test]
    public void Positive_GetPath()
    {
        Vector3 initialPos = Vector3.zero;
        Vector3 finalPos = new Vector3(2, 2, 0);

        List<Node> path = AStarPathfinding.Instance.GetPath(initialPos, finalPos, _team);

        Assert.AreEqual(path.Count, 6);
    }
}
