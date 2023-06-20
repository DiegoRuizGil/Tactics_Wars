using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class AStarPathfinding
{
    private static AStarPathfinding _instance;

    private List<Node> _openList;
    private List<Node> _closedList;

    private const int MOVE_COST = 10;

    private AStarPathfinding() { }

    public static AStarPathfinding Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AStarPathfinding();
            }
            return _instance;
        }
    }

    public List<Node> GetPath(Vector3 startPosition, Vector3 finalPosition, TeamEnum team)
    {
        ResetNodes();

        Node startNode = Grid.Instance.GetNode(startPosition);
        Node finalNode = Grid.Instance.GetNode(finalPosition);

        _openList = new List<Node> { startNode };
        _closedList = new List<Node>();

        startNode.GCost = 0;
        startNode.HCost = GetHeuristic(startPosition, finalPosition);

        List<Node> sortedOpenNodes;
        Node currentNode = null;
        List<Node> neighbours;

        while (_openList.Count > 0 || currentNode == finalNode)
        {
            sortedOpenNodes = _openList.OrderBy(node => node.FCost).ToList();
            currentNode = sortedOpenNodes[0];
            _closedList.Add(currentNode);
            _openList.Remove(currentNode);

            // Stop condition
            if (currentNode == finalNode)
            {
                break;
            }

            neighbours = GetNeighbours(startNode, finalNode, currentNode, team);
            foreach (Node nextNode in neighbours)
            {
                if (_openList.Contains(nextNode) && (currentNode.GCost + MOVE_COST) < nextNode.GCost)
                {
                    nextNode.NodeParent = currentNode;
                    nextNode.GCost = currentNode.GCost + MOVE_COST;
                    nextNode.HCost = GetHeuristic(nextNode, finalNode);
                }
                else if (_closedList.Contains(nextNode) && (currentNode.GCost + MOVE_COST) < nextNode.GCost)
                {
                    nextNode.NodeParent = currentNode;
                    nextNode.GCost = currentNode.GCost + MOVE_COST;
                    nextNode.HCost = GetHeuristic(nextNode, finalNode);

                    _closedList.Remove(nextNode);
                    _openList.Add(nextNode);
                }
                else if (!_openList.Contains(nextNode) && !_closedList.Contains(nextNode))
                {
                    nextNode.NodeParent = currentNode;
                    nextNode.GCost = currentNode.GCost + MOVE_COST;
                    nextNode.HCost = GetHeuristic(nextNode, finalNode);

                    _openList.Add(nextNode);
                }
            }
        }

        List<Node> path = new List<Node>();
        if (finalNode.NodeParent != null) // path found
        {
            path = GetNodePath(startNode, finalNode, team);
        }

        return path;
    }

    private List<Node> GetAStarPath(Node node)
    {
        List<Node> path = new List<Node>();
        Node currentNode = node;

        path.Add(currentNode);
        while (currentNode.NodeParent != null)
        {
            currentNode = currentNode.NodeParent;
            path.Add(currentNode);
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNodePath(Node startNode, Node finalNode, TeamEnum team)
    {
        List<Node> aStarPath = GetAStarPath(finalNode);
        return GetNodePath(aStarPath, startNode, finalNode, team);
    }

    private List<Node> GetNodePath(List<Node> aStarPath, Node startNode, Node finalNode, TeamEnum team)
    {
        List<Node> path = new List<Node>();

        Entity topEntity;
        foreach (Node node in aStarPath)
        {
            if (node == startNode)
                continue;

            topEntity = node.GetTopEntity();
            if (topEntity != null)
            {
                if (node == finalNode && topEntity.Team != team)
                    continue;
            }

            path.Add(node);
        }

        return path;
    }

    private void ResetNodes()
    {
        foreach (Node node in Grid.Instance.Nodes.Cast<Node>())
        {
            node.GCost = int.MaxValue / 2;
            node.HCost = int.MaxValue / 2;
            node.NodeParent = null;
        }
    }

    private int GetHeuristic(Node startNode, Node finalNode)
    {
        Vector3 startPosition = Grid.Instance.GetNodeWorldPosition(startNode.GridX, startNode.GridY);
        Vector3 finalPosition = Grid.Instance.GetNodeWorldPosition(finalNode.GridX, finalNode.GridY);

        return GetHeuristic(startPosition, finalPosition);
    }

    private int GetHeuristic(Vector3 startPosition, Vector3 finalPosition)
    {
        float xValue = Mathf.Abs(finalPosition.x - startPosition.x);
        float yValue = Mathf.Abs(finalPosition.y - startPosition.y);

        return Mathf.FloorToInt(xValue + yValue);
    }

    private List<Node> GetNeighbours(Node startNode, Node finalNode, Node currentNode, TeamEnum team)
    {
        List<Node> neighbours = new List<Node>();

        Entity topEntityFinalNode = finalNode.GetTopEntity();
        Entity topEntity;
        foreach (Node node in currentNode.Neighbours)
        {
            if (node == startNode)
                continue;

            if (node == finalNode)
            {
                neighbours.Add(node);
                continue;
            }

            topEntity = node.GetTopEntity();
            if (topEntity != null)
            {
                // enemy entities are walls
                if (topEntity.Team != team)
                    continue;

                if (topEntityFinalNode != null && topEntityFinalNode.Team != team)
                {
                    // avoid path with team unit next to finalnode
                    if (topEntity.Team == team && topEntity is Unit && finalNode.Neighbours.Contains(node))
                        continue;
                }
            }
            
            neighbours.Add(node);
        }

        return neighbours;
    }
}
