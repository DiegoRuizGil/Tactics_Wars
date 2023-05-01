using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid
{
    public static Grid Instance { get; set; }

    private readonly Node[,] _nodes;
    private readonly int _width;
    private readonly int _height;
    private readonly int _cellSize;
    private readonly Vector3 _startPosition;

    public int Width { get { return _width; } }
    public int Height { get { return _height; } }

    public Grid(int width, int height, int cellSize, Vector3 startPoint)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _startPosition = startPoint;
        _nodes = new Node[width, height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _nodes[x, y] = new Node(GetNodeWorldPosition(x, y), x, y);
            }
        }
    }

    public Vector3 GetNodeWorldPosition(Node node)
    {
        return GetNodeWorldPosition(node.GridX, node.GridY);
    }

    public Vector3 GetNodeWorldPosition(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return new Vector3(x * _cellSize, y * _cellSize, 0) + _startPosition;
        else
            throw new GridOutOfBoundsException(x, y);
    }

    public Vector3 GetNodeWorldPosition(Vector3 position)
    {
        if (GetNode(position) == null)
            throw new GridOutOfBoundsException(position.x, position.y);
        return GetNodeWorldPosition(GetNode(position));
    }

    public Vector3Int GetNodeWorldIntPosition(int x, int y)
    {
        return Vector3Int.FloorToInt(GetNodeWorldPosition(x, y));
    }

    public Node GetNode(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return _nodes[x, y];
        else
            return null;
    }

    public Node GetNode(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x - _startPosition.x) / _cellSize;
        int y = Mathf.FloorToInt(position.y - _startPosition.y) / _cellSize;

        return GetNode(x, y);
    }

    public List<Node> GetNodesWithResourceType(ResourceType resourceType)
    {
        return _nodes.OfType<Node>().Where(node => node.Resource == resourceType).ToList();
    }

    public bool CheckIfSameNode(Vector3 pos1, Vector3 pos2)
    {
        return GetNode(pos1).Equals(GetNode(pos2));
    }

    public void SetNodesNeighbours()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _nodes[x, y].Neighbours = GetNodeNeighbours(_nodes[x, y]);
            }
        }
    }

    private List<Node> GetNodeNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Up
        Node up = GetNode(node.GridX, node.GridY + 1);
        if (up != null)
        {
            if (!up.IsWall)
                neighbours.Add(up);
        }

        // Down
        Node down = GetNode(node.GridX, node.GridY - 1);
        if (down != null)
        {
            if (!down.IsWall)
                neighbours.Add(down);
        }

        // Left
        Node left = GetNode(node.GridX - 1, node.GridY);
        if (left != null)
        {
            if (!left.IsWall)
                neighbours.Add(left);
        }

        // Right
        Node right = GetNode(node.GridX + 1, node.GridY);
        if (right != null)
        {
            if (!right.IsWall)
                neighbours.Add(right);
        }

        return neighbours;
    }
}
