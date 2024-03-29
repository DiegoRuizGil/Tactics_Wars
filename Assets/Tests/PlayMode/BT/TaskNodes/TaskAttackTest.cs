using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class TaskAttackTest
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
        Unit unit = An.Unit.WithTeam(TeamEnum.RED);
        Unit target = An.Unit.WithTeam(TeamEnum.BLUE);

        TaskAttack taskNode = new TaskAttack(_tree, unit);

        Unit targetInstance = GameManager.Instance.InstantiateUnit(target, Vector3.zero, TeamEnum.BLUE);

        _tree.SetData("target", targetInstance);

        taskNode.Evaluate();
        Assert.AreEqual(taskNode.State, TreeNodeState.RUNNING);

        yield return null;
    }
}
