using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class TaskMoveTest
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
        Grid.Instance.SetNodesNeighbours();
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
        Unit unit = An.Unit;
        GameManager.Instance.InstantiateUnit(unit, Vector3.zero, TeamEnum.BLUE);

        _tree.SetData("targetPosition", new Vector3(4, 5, 0));

        TaskMove taskNode = new TaskMove(_tree, unit);

        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.RUNNING, taskNode.State);

        yield return null;
    }
}
