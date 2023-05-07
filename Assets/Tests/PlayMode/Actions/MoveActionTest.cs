using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MoveActionTest
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
    public IEnumerator Negative_Execute()
    {
        List<Vector3> positions = new List<Vector3> { new Vector3(_width, _height) };
        Unit unit = An.Unit.WithPosition(new Vector3(_width - 1, _height - 1, 0f));
        Node node = Grid.Instance.GetNode(unit.transform.position);
        node.AddEntity(unit);

        MoveAction action = new MoveAction(unit, positions, 1);
        action.Execute();
        Assert.IsNotNull(node.GetTopEntity());

        action = new MoveAction(unit, new List<Vector3>(), 1);
        action.Execute();
        Assert.IsNotNull(node.GetTopEntity());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_Execute()
    {
        List<Vector3> positions = new List<Vector3>
        {
            new Vector3(_width - 2, _height - 3, 0f),
            new Vector3(_width - 2, _height - 2, 0f)
        };
        int movementSpeed = 1;
        Unit unit = An.Unit.WithPosition(new Vector3(_width - 3, _height - 3, 0f));
        Node node = Grid.Instance.GetNode(unit.transform.position);
        node.AddEntity(unit);
        Node newNode = Grid.Instance.GetNode(_width - 2, _height - 2);

        yield return null;

        MoveAction action = new MoveAction(unit, positions, movementSpeed);
        action.Execute();

        yield return new WaitForSeconds((movementSpeed * positions.Count) + 1f);

        Assert.IsTrue(unit.HasMoved);
        Assert.IsNull(node.GetTopEntity());
        Assert.IsNotNull(newNode.GetTopEntity());
    }
}
