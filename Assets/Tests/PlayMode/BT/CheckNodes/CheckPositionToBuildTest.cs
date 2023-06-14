using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class CheckPositionToBuildTest
{
    private UnitBT _tree;

    [SetUp]
    public void SetUp()
    {
        GameObject go = new GameObject();
        _tree = go.AddComponent<UnitBT>();

        GameManager gameManager = A.GameManager;

        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);
        Grid.Instance.SetNodesNeighbours();

        Grid.Instance.GetNode(1, 0).Resource = ResourceType.FOOD;
        Grid.Instance.GetNode(5, 0).Resource = ResourceType.FOOD;

        Grid.Instance.GetNode(0, 1).Resource = ResourceType.GOLD;
        Grid.Instance.GetNode(0, 10).Resource = ResourceType.GOLD;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            GameObject.DestroyImmediate(obj);
        }
    }

    [UnityTest]
    public IEnumerator Positive_Evaluate()
    {
        Building urbanCenter = A.Building.WithType(BuildingType.URBAN_CENTER);
        Building windmill = A.Building.WithType(BuildingType.WINDMILL);
        Unit unit = An.Unit.WithPosition(Vector3.zero).WithTeam(TeamEnum.BLUE);
        unit.HasMoved = true;

        CheckPositionToBuild checkNode = new CheckPositionToBuild(_tree, unit);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE);

        unit.HasMoved = false;
        _tree.SetData("targetPosition", Vector3.zero);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS);

        _tree.ClearData("targetPosition");

        _tree.SetData("buildMode", ResourceType.NONE);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE); // urban center is null

        GameManager.Instance.InstantiateBuilding(urbanCenter, new Vector3(2, 0, 0), TeamEnum.BLUE);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS);

        _tree.ClearData("targetPosition");

        _tree.SetData("buildMode", ResourceType.FOOD);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS); // windmill

        _tree.ClearData("targetPosition");

        GameManager.Instance.InstantiateBuilding(windmill, new Vector3(1, 0, 0), TeamEnum.BLUE);
        _tree.SetData("buildMode", ResourceType.FOOD);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS); // farm

        _tree.ClearData("targetPosition");

        _tree.SetData("buildMode", ResourceType.GOLD);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS); // goldmine

        yield return null;
    }
}
