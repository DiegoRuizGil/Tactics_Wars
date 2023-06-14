using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GenerateUnitActionTest
{
    [SetUp]
    public void SetUp()
    {
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
    public IEnumerator Positive_Execute()
    {
        GameObject parent = new GameObject();
        TeamEnum team = TeamEnum.BLUE;
        A.GameManager.WithUnitParent(team, parent.transform);
        
        Unit unit = An.Unit;
        UnitInfoSO info = An.UnitInfoSO.WithUnit(unit).WithFoodAmount(0).WithGoldAmount(0);

        yield return null;

        Vector3 position = Vector3.zero;
        Node node = Grid.Instance.GetNode(position);

        GenerateUnitAction action = new GenerateUnitAction(info, position, team);
        action.Execute();

        Assert.AreEqual(1, parent.transform.childCount);

        Unit unitInstance = parent.transform.GetChild(0).GetComponent<Unit>();
        Assert.AreEqual(position, unitInstance.transform.position);
        Assert.AreEqual(unitInstance, node.GetTopEntity());
    }
}
