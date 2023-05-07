using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class TaskFinalizeTest
{
    private UnitBT _tree;

    [SetUp]
    public void SetUp()
    {
        GameObject go = new GameObject();
        _tree = go.AddComponent<UnitBT>();

        A.GameManager.WithFoodAmount(TeamEnum.BLUE, 0);
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

        TaskFinalize taskNode = new TaskFinalize(_tree, unit);

        taskNode.Evaluate();
        Assert.IsTrue(unit.HasFinished);
        Assert.AreEqual(taskNode.State, TreeNodeState.SUCCESS);

        yield return null;
    }
}
