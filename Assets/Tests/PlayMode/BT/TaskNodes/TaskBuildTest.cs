using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using BehaviourTree;

public class TaskBuildTest
{
    private VillagerBT _tree;
    private Unit _unit;

    [SetUp]
    public void SetUp()
    {
        GameManager gameManager = A.GameManager;
        gameManager.UpdateResources(TeamEnum.RED, 10000, 10000);

        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);
        Grid.Instance.SetNodesNeighbours();
        Grid.Instance.GetNode(0, 1).Resource = ResourceType.GOLD;
        Grid.Instance.GetNode(1, 0).Resource = ResourceType.FOOD;

        string prefabName = "Aldeano";
        string path = "Assets/Prefabs/Entities/Units/" + prefabName + ".prefab";

        GameObject villagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        _unit = villagerPrefab.GetComponent<Unit>();
        _tree = villagerPrefab.GetComponent<VillagerBT>();
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
        Building building = A.Building.WithType(BuildingType.URBAN_CENTER);
        GameManager.Instance.InstantiateBuilding(building, new Vector3(2, 2, 0), TeamEnum.RED);

        Unit unitInstance = GameManager.Instance.InstantiateUnit(_unit, Vector3.zero, TeamEnum.RED);

        TaskBuild taskNode = new TaskBuild(_tree, unitInstance);

        // can not build
        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.FAILURE, taskNode.State);

        // resource building
        unitInstance.transform.position = Vector3.up;
        unitInstance.SetEntityInGrid();

        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.SUCCESS, taskNode.State);

        // unit building
        unitInstance.transform.position = new Vector3(1, 2, 0);
        unitInstance.SetEntityInGrid();
        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.SUCCESS, taskNode.State);

        unitInstance.transform.position = new Vector3(2, 3, 0);
        unitInstance.SetEntityInGrid();
        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.SUCCESS, taskNode.State);

        unitInstance.transform.position = new Vector3(3, 2, 0);
        unitInstance.SetEntityInGrid();
        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.SUCCESS, taskNode.State);

        unitInstance.transform.position = new Vector3(2, 1, 0);
        unitInstance.SetEntityInGrid();
        taskNode.Evaluate();
        Assert.AreEqual(TreeNodeState.SUCCESS, taskNode.State);

        yield return null;
    }

    
}
