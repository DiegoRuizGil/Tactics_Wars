using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class CheckCanDoActionsTest
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
        Unit unit = An.Unit.WithTeam(TeamEnum.RED);
        unit.HasFinished = true;

        CheckCanDoActions checkNode = new CheckCanDoActions(_tree, unit);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE);

        unit.Team = TeamEnum.BLUE;
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE);


        unit.HasFinished = false;
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS);

        yield return null;
    }
}
