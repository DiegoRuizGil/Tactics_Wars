using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pathfinding
{
    private static Pathfinding _instance;
    private HashSet<Node> _nodesToReset = new HashSet<Node>();
    private Node _initialNode;

    private Pathfinding() { }

    public static Pathfinding Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Pathfinding();
            }
            return _instance;
        }
    }

    private HashSet<Node> GetArea(Vector3 initialPos, int moveCost, int maxDistance, Func<Node, HashSet<Node>> getNeighbours)
    {
        HashSet<Node> area = new HashSet<Node>();
        ResetVisitedNodes();
        _initialNode = Grid.Instance.GetNode(initialPos);

        _nodesToReset.Add(_initialNode);

        HashSet<Node> neighbours = getNeighbours(_initialNode);
        HashSet<Node> tempNeighbours = new HashSet<Node>();

        while (neighbours.Count > 0)
        {
            foreach (Node node in neighbours)
            {
                int parentCost = node.NodeParent.DistanceCost;

                node.DistanceCost = parentCost + moveCost;
                area.Add(node);
                if (node.DistanceCost < maxDistance)
                {
                    tempNeighbours.UnionWith(getNeighbours(node));
                }
            }
            neighbours = tempNeighbours;
            tempNeighbours = new HashSet<Node>();
        }

        return area;
    }

    #region Move Action
    public List<Vector3> GetMovementArea(Vector3 initialPos, int moveCost, int maxDistance)
    {
        List<Vector3> walkableArea = new List<Vector3>();

        if (moveCost < 1)
            return walkableArea;
        if (maxDistance < 1)
            return walkableArea;

        foreach (Node node in GetArea(initialPos, moveCost, maxDistance, GetNeighboursMoveAction))
        {
            if (node.GetTopEntity() is not Unit)
                walkableArea.Add(node.Position);
        }

        return walkableArea;
    }

    public List<Vector3> CalculatePositionsPath(Vector3 finalPosition)
    {
        List<Vector3> positionsPath = new List<Vector3>();

        List<Node> nodePath = CalculateNodePath(finalPosition);
        foreach (Node node in nodePath)
        {
            positionsPath.Add(node.Position);
        }

        return positionsPath;
    }

    private List<Node> CalculateNodePath(Vector3 finalPosition)
    {
        Node node = Grid.Instance.GetNode(finalPosition);
        return CalculateNodePath(node);
    }

    private List<Node> CalculateNodePath(Node node)
    {
        if (node.NodeParent == null)
        {
            return new List<Node> { node };
        }
        else
        {
            List<Node> path = new List<Node>();
            path.AddRange(CalculateNodePath(node.NodeParent));
            path.Add(node);
            return path;
        }
    }

    private HashSet<Node> GetNeighboursMoveAction(Node currentNode)
    {
        HashSet<Node> neighbours = new HashSet<Node>();

        Entity entityInNode = _initialNode.GetTopEntity();
        foreach (Node node in currentNode.Neighbours)
        {
            if (node.GetTopEntity() != null)
            {
                if (node.GetTopEntity().Team != (entityInNode != null ? entityInNode.Team : null))
                    continue;
            }
            
            if (!node.Equals(_initialNode) && !_nodesToReset.Contains(node))
            {
                node.NodeParent = currentNode;
                neighbours.Add(node);
                _nodesToReset.Add(node);
            }
        }

        return neighbours;
    }
    #endregion

    #region Attack Action
    public List<Vector3> GetAttackArea(Vector3 initialPos, int attackRange)
    {
        List<Vector3> attackArea = new List<Vector3>();

        if (attackRange < 1)
            return attackArea;

        foreach (Node node in GetArea(initialPos, 1, attackRange, GetNeighboursAttackAction))
        {
            if (node.GetTopEntity() == null)
                continue;

            if (node.GetTopEntity().Team != _initialNode.GetTopEntity().Team)
                attackArea.Add(node.Position);
        }

        return attackArea;
    }

    private HashSet<Node> GetNeighboursAttackAction(Node currentNode)
    {
        HashSet<Node> neighbours = new HashSet<Node>();
        foreach (Node node in currentNode.Neighbours)
        {
            if (!node.Equals(_initialNode) && !_nodesToReset.Contains(node))
            {
                node.NodeParent = currentNode;
                neighbours.Add(node);
                _nodesToReset.Add(node);
            }
        }

        return neighbours;
    }
    #endregion

    private void ResetVisitedNodes()
    {
        foreach (Node node in _nodesToReset)
        {
            node.NodeParent = null;
            node.DistanceCost = 0;
        }

        _nodesToReset = new HashSet<Node>();
    }
}
