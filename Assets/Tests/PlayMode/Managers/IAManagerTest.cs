using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class IAManagerTest
{
    private TeamEnum _team;

    private const string BUILDING_PREFABS_BASE_PATH = "Assets/Prefabs/Entities/Buildings/";

    [SetUp]
    public void SetUp()
    {
        _team = TeamEnum.RED;

        GameManager gameManager = A.GameManager;
        gameManager.UpdateResources(TeamEnum.RED, 10000, 10000);

        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);
        Grid.Instance.SetNodesNeighbours();

        // set urban center
        Building urbanCenter = AssetDatabase.LoadAssetAtPath<Building>(
            BUILDING_PREFABS_BASE_PATH + "Centro Urbano.prefab"
        );
        GameManager.Instance.InstantiateBuilding(urbanCenter, new Vector3(2, 2, 0), _team);

        // set unit building
        Building unitBuilding = AssetDatabase.LoadAssetAtPath<Building>(
            BUILDING_PREFABS_BASE_PATH + "UnitBuildings/Establo.prefab"
        );
        GameManager.Instance.InstantiateBuilding(unitBuilding, new Vector3(3, 2, 0), _team);
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
    public IEnumerator Positive_ManageEntities()
    {
        Unit villager = An.Unit.WithUnitType(UnitType.ALDEANO);
        villager.gameObject.AddComponent<TestBT>().enabled = false;

        Unit villagerA = GameManager.Instance.InstantiateUnit(villager, Vector3.zero, _team);
        Unit villagerB = GameManager.Instance.InstantiateUnit(villager, Vector3.up, _team);

        IAManager iaManager = An.IAManager;

        iaManager.ManageEntities(_team);

        Assert.AreEqual(ResourceType.FOOD, villagerA.GetComponent<TestBT>().GetData("buildMode"));

        yield return null;

        Assert.IsTrue(villagerA.HasFinished);

        yield return null;

        Assert.AreEqual(ResourceType.GOLD, villagerB.GetComponent<TestBT>().GetData("buildMode"));

        yield return null;

        Assert.IsTrue(villagerA.HasFinished);

        yield return null;
        yield return null;

        Unit newVillager = Grid.Instance.GetNode(new Vector3(2, 2, 0)).GetTopEntity() as Unit;
        Unit newUnit = Grid.Instance.GetNode(new Vector3(3, 2, 0)).GetTopEntity() as Unit;

        Assert.IsNotNull(newVillager);
        Assert.AreEqual(UnitType.ALDEANO, newVillager.UnitType);

        Assert.IsNotNull(newUnit);
    }
}
