using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class CheckUnitInBuildPositionTest
{
    private UnitBT _tree;

    [SetUp]
    public void SetUp()
    {
        GameObject go = new GameObject();
        _tree = go.AddComponent<UnitBT>();

        A.GameManager.WithFoodAmount(TeamEnum.BLUE, 0);

        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);
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
        Unit unit = An.Unit.WithPosition(Vector3.zero);

        CheckUnitInBuildPosition checkNode = new CheckUnitInBuildPosition(_tree, unit);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE); // no position in tree data

        _tree.SetData("targetPosition", Vector3.up);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE); // unit position not equal target position

        _tree.SetData("targetPosition", Vector3.zero);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS); // unit position equal target position

        _tree.SetData("target", An.Unit);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE); // unit has set a target to attack

        yield return null;
    }
}
