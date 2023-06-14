using NUnit.Framework;
using UnityEngine;

public class GridTest
{
    private int _width;
    private int _height;
    private int _cellSize;
    private Vector3 _originPosition;

    [SetUp]
    public void SetUp()
    {
        _width = 6;
        _height = 11;
        _cellSize = 1;
        _originPosition = Vector3.zero;

        Grid.Instance = new Grid(_width, _height, _cellSize, _originPosition);
    }

    [Test]
    public void GetNodeWorldPosition_OutOfBoundsPosition_Exception()
    {
        int x = -1;
        int y = -2;
        Assert.Throws<GridOutOfBoundsException>(() => Grid.Instance.GetNodeWorldPosition(x, y));

        x = _width;
        y = _height;
        Assert.Throws<GridOutOfBoundsException>(() => Grid.Instance.GetNodeWorldPosition(x, y));

        Node node = new Node(Vector3.zero, x, y);
        Assert.Throws<GridOutOfBoundsException>(() => Grid.Instance.GetNodeWorldPosition(node));

        Vector3 position = new Vector3(_width + 1, _height + 1, 0f) + _originPosition;
        Assert.Throws<GridOutOfBoundsException>(() => Grid.Instance.GetNodeWorldPosition(position));
    }

    [Test]
    public void GetNodeWorldPosition_GridNode_NodePosition()
    {
        Vector3 expectedPosition = new Vector3((_width - 1) * _cellSize, (_height - 1) * _cellSize, 0f);

        Node node = Grid.Instance.GetNode(_width - 1, _height - 1);

        Assert.AreEqual(expectedPosition, Grid.Instance.GetNodeWorldPosition(node));
        Assert.AreEqual(expectedPosition, Grid.Instance.GetNodeWorldPosition(node.Position));
        Assert.AreEqual(expectedPosition, Grid.Instance.GetNodeWorldPosition(node.GridX, node.GridY));
    }

    [Test]
    public void GetNode_OutOfBoundsPosition_NullValue()
    {
        int x = -1;
        int y = -2;
        Assert.IsNull(Grid.Instance.GetNode(x, y));

        x = _width;
        y = _height;
        Assert.IsNull(Grid.Instance.GetNode(x, y));

        Vector3 position = new Vector3(_width + 1, _height + 1, 0f) + _originPosition;
        Assert.IsNull(Grid.Instance.GetNode(position));
    }

    [Test]
    public void GetNode_NodePosition_NotNull()
    {
        int x = _width - 1;
        int y = _height - 1;
        Assert.IsNotNull(Grid.Instance.GetNode(x, y));

        Vector3 position = new Vector3(_width - 1, _height - 1, 0f) + _originPosition;
        Assert.IsNotNull(Grid.Instance.GetNode(position));
    }

    [Test]
    public void SetNodesNeighbours_Void_CorrectNeighbours()
    {
        Assert.GreaterOrEqual(_width * _height, 9);
        Grid.Instance.SetNodesNeighbours();

        Node neighbour = Grid.Instance.GetNode(1, 0);

        Node node = Grid.Instance.GetNode(0, 0);
        Assert.AreEqual(2, node.Neighbours.Count);
        Assert.IsTrue(node.Neighbours.Contains(neighbour));

        node = Grid.Instance.GetNode(1, 1);
        Assert.AreEqual(4, node.Neighbours.Count);
        Assert.IsTrue(node.Neighbours.Contains(neighbour));
    }
}
