using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTest
{
    private TeamEnum _team;

    [SetUp]
    public void SetUp()
    {
        _team = TeamEnum.BLUE;
        GameManager gameManager = A.GameManager;

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
    public IEnumerator Positive_UpdateResources()
    {
        yield return null;

        int initialFood = GameManager.Instance.FoodResources[_team];
        int initialGold = GameManager.Instance.GoldResources[_team];

        int foodAmount = 450;
        int goldAmount = 450;

        Assert.IsTrue(GameManager.Instance.UpdateResources(_team, foodAmount, goldAmount));
        Assert.AreEqual(GameManager.Instance.FoodResources[_team], initialFood + foodAmount);
        Assert.AreEqual(GameManager.Instance.GoldResources[_team], initialGold + goldAmount);
    }

    [UnityTest]
    public IEnumerator Negative_UpdateResources()
    {
        yield return null;

        int initialFood = GameManager.Instance.FoodResources[_team];
        int initialGold = GameManager.Instance.GoldResources[_team];

        int foodAmount = -1 * (initialFood + 10);
        int goldAmount = -1 * (initialGold + 10);

        Assert.IsFalse(GameManager.Instance.UpdateResources(_team, foodAmount, goldAmount));
        Assert.AreEqual(GameManager.Instance.FoodResources[_team], initialFood);
        Assert.AreEqual(GameManager.Instance.GoldResources[_team], initialGold);
    }

    [UnityTest]
    public IEnumerator Positive_InstantiateEntity()
    {
        yield return null;

        Building building = A.Building;
        Vector3 buildingPos = Vector3.zero;

        Unit unit = An.Unit;
        Vector3 unitPos = Vector3.up;

        Building buildingInstance = GameManager.Instance.InstantiateEntity(building, buildingPos, _team) as Building;
        Unit unitInstance = GameManager.Instance.InstantiateEntity(unit, unitPos, _team) as Unit;

        yield return null;

        Node buildingNode = Grid.Instance.GetNode(buildingPos);
        Node unitNode = Grid.Instance.GetNode(unitPos);

        Assert.AreEqual(buildingNode.GetTopEntity(), buildingInstance);
        Assert.AreEqual(unitNode.GetTopEntity(), unitInstance);
    }

    [UnityTest]
    public IEnumerator Positive_RemoveEntity()
    {
        yield return null;

        Vector3 buildingPos = Vector3.zero;
        Building building = A.Building.WithPosition(buildingPos);
        building.SetEntityInGrid();

        Vector3 unitPos = Vector3.up;
        Unit unit = An.Unit.WithPosition(unitPos);
        unit.SetEntityInGrid();

        yield return null;

        Node buildingNode = Grid.Instance.GetNode(buildingPos);
        Node unitNode = Grid.Instance.GetNode(unitPos);

        Assert.AreEqual(buildingNode.GetTopEntity(), building);
        Assert.AreEqual(unitNode.GetTopEntity(), unit);

        GameManager.Instance.RemoveBuilding(building);
        GameManager.Instance.RemoveUnit(unit);

        Assert.IsNull(buildingNode.GetTopEntity());
        Assert.IsNull(unitNode.GetTopEntity());
    }
}
