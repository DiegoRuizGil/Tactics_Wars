using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BuildActionTest
{
    private int _width;
    private int _height;
    private int _cellSize;

    [SetUp]
    public void SetUp()
    {
        _width = 6;
        _height = 11;
        _cellSize = 1;
        Grid.Instance = new Grid(_width, _height, _cellSize, Vector3.zero);
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
    public IEnumerator Positive_Execute()
    {
        GameObject parent = new GameObject();
        TeamEnum team = TeamEnum.BLUE;
        GameManager gameManager = A.GameManager.WithBuildingParent(team, parent.transform);
        Unit unit = An.Unit.WithPosition(new Vector3(_width - 1, _height - 1, 0f));
        Building building = A.Building;
        BuildingInfoSO info = A.BuildingInfoSO.WithBuilding(building).WithFoodAmount(0).WithGoldAmount(0);

        yield return null;

        Node node = Grid.Instance.GetNode(unit.transform.position);
        node.AddEntity(unit);

        BuildAction action = new BuildAction(info, unit, team);
        action.Execute();

        Assert.AreEqual(1, parent.transform.childCount);

        Building buildingInstance = parent.transform.GetChild(0).GetComponent<Building>();

        Assert.AreEqual(unit.transform.position, buildingInstance.transform.position);
    }
}
