using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NodeTest
{
    private Unit _unit;
    private Building _building;

    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        _unit = gameObject.AddComponent<Unit>();
        _building = gameObject.AddComponent<Building>();
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
    public IEnumerator Negative_AddEntity()
    {
        Node node = A.Node.WithUnit(_unit).WithBuilding(_building);
        GameObject gameObject = new GameObject();
        Unit unit = gameObject.AddComponent<Unit>();

        Assert.IsFalse(node.AddEntity(unit));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Negative_GetEntity()
    {
        Node node = A.Node;

        Assert.IsNull(node.GetEntity(0));
        Assert.IsNull(node.GetEntity(1));
        Assert.IsNull(node.GetEntity(2));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Negative_GetTopEntity()
    {
        Node node = A.Node;

        Assert.IsNull(node.GetTopEntity());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Negative_RemoveTopEntity()
    {
        Node node = A.Node;

        Assert.IsNull(node.RemoveTopEntity());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_AddEntity()
    {
        Node node = A.Node;

        Assert.IsTrue(node.AddEntity(_unit));
        Assert.IsTrue(node.AddEntity(_building));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_GetEntity()
    {
        Node node = A.Node.WithUnit(_unit).WithBuilding(_building);

        Assert.IsNotNull(node.GetEntity(0));
        Assert.AreEqual(_building, node.GetEntity(0));
        Assert.IsNotNull(node.GetEntity(1));
        Assert.AreEqual(_unit, node.GetEntity(1));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_GetTopEntity()
    {
        Node node = A.Node.WithUnit(_unit);

        Assert.IsNotNull(node.GetTopEntity());
        Assert.AreEqual(_unit, node.GetTopEntity());

        node = A.Node.WithBuilding(_building);

        Assert.IsNotNull(node.GetTopEntity());
        Assert.AreEqual(_building, node.GetTopEntity());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_RemoveTopEntity()
    {
        Node node = A.Node.WithUnit(_unit).WithBuilding(_building);

        Assert.AreEqual(_unit, node.RemoveTopEntity());
        Assert.AreEqual(_building, node.RemoveTopEntity());

        yield return null;
    }
}
