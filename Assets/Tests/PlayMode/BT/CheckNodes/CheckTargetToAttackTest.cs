using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BehaviourTree;

public class CheckTargetToAttackTest
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
        Unit unit = An.Unit.WithTeam(TeamEnum.RED).WithUnitType(UnitType.ALABARDERO).WithPosition(Vector3.zero);
        unit.HasMoved = true;

        CheckTargetToAttack checkNode = new CheckTargetToAttack(_tree, unit);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE);

        unit.HasMoved = false;

        _tree.ClearData("target");
        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.FAILURE); // no enemy entity in map

        Unit enemyUnit = An.Unit.WithWeaknesess(UnitType.ALABARDERO);
        Building enemyBuilding = A.Building;

        GameManager.Instance.InstantiateUnit(enemyUnit, new Vector3(2, 0, 0), TeamEnum.BLUE);
        GameManager.Instance.InstantiateBuilding(enemyBuilding, new Vector3(3, 1, 0), TeamEnum.BLUE);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS);

        checkNode.Evaluate();
        Assert.AreEqual(checkNode.State, TreeNodeState.SUCCESS);

        yield return null;
    }
}
