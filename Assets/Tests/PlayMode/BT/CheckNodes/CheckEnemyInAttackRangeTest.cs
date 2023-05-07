using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class CheckEnemyInAttackRangeTest
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
        Unit unit = An.Unit.WithTeam(TeamEnum.BLUE).WithPosition(Vector3.zero);
        Unit target = An.Unit.WithTeam(TeamEnum.RED).WithPosition(new Vector3(0, 2, 0));

        CheckEnemyInAttackRange checkNode = new CheckEnemyInAttackRange(_tree, unit);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE);

        _tree.SetData("target", target);
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE);

        target.transform.position = Vector3.up;
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS);

        yield return null;
    }
}
