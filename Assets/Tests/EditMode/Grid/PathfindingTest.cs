using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PathfindingTest
{
    [SetUp]
    public void SetUp()
    {
        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);
        Grid.Instance.SetNodesNeighbours();
    }

    [Test]
    public void Negative_GetMovementArea()
    {
        Vector3 initialPos = Vector3.zero;
        int moveCost = -1;
        int maxDistance = -1;

        List<Vector3> area = Pathfinding.Instance.GetMovementArea(initialPos, moveCost, maxDistance);

        Assert.AreEqual(0, area.Count);
    }

    [Test]
    public void Negative_GetAttackArea()
    {
        Vector3 initialPos = Vector3.zero;
        int attackRange = -1;

        List<Vector3> area = Pathfinding.Instance.GetAttackArea(initialPos, attackRange);

        Assert.AreEqual(0, area.Count);
    }

    [Test]
    public void Positive_GetMovementArea()
    {
        Vector3 initialPos = new Vector3(2f, 3f, 0f);
        Vector3 enemyPos = new Vector3(3f, 3f, 0f);
        int moveCost = 1;
        int maxDistance = 2;

        Unit unit = An.Unit.WithPosition(initialPos).WithTeam(TeamEnum.BLUE);
        Unit enemy = An.Unit.WithPosition(enemyPos).WithTeam(TeamEnum.RED);

        Grid.Instance.GetNode(initialPos).AddEntity(unit);
        Grid.Instance.GetNode(enemyPos).AddEntity(enemy);

        List<Vector3> area = Pathfinding.Instance.GetMovementArea(initialPos, moveCost, maxDistance);

        Assert.AreEqual(10, area.Count);
        Assert.IsFalse(area.Contains(enemyPos));
        Assert.IsFalse(area.Contains(initialPos));
    }

    [Test]
    public void Positive_CalculatePositionsPath()
    {
        Vector3 initialPos = Vector3.zero;
        Vector3 finalPos = new Vector3(2f, 0f, 0f);
        int moveCost = 1;
        int maxDistance = 2;

        Pathfinding.Instance.GetMovementArea(initialPos, moveCost, maxDistance);

        List<Vector3> path = Pathfinding.Instance.CalculatePositionsPath(finalPos);

        Assert.AreEqual(3, path.Count);
        Assert.IsTrue(path.Contains(Vector3.right));
    }

    [Test]
    public void Positive_GetAttackArea()
    {
        Vector3 initialPos = new Vector3(2f, 3f, 0f);
        Vector3 enemy1Pos = new Vector3(3f, 3f, 0f);
        Vector3 enemy2Pos = new Vector3(2f, 5f, 0f);
        int attackRange = 2;

        Unit unit = An.Unit.WithPosition(initialPos).WithTeam(TeamEnum.BLUE);
        Unit enemy1 = An.Unit.WithPosition(enemy1Pos).WithTeam(TeamEnum.RED);
        Unit enemy2 = An.Unit.WithPosition(enemy2Pos).WithTeam(TeamEnum.RED);

        Grid.Instance.GetNode(initialPos).AddEntity(unit);
        Grid.Instance.GetNode(enemy1Pos).AddEntity(enemy1);
        Grid.Instance.GetNode(enemy2Pos).AddEntity(enemy2);

        List<Vector3> area = Pathfinding.Instance.GetAttackArea(initialPos, attackRange);

        Assert.AreEqual(2, area.Count);
        Assert.IsTrue(area.Contains(enemy1Pos));
        Assert.IsTrue(area.Contains(enemy2Pos));
        Assert.IsFalse(area.Contains(initialPos));
    }
}
