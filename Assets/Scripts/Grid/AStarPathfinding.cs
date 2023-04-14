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

    public List<Vector3> GetPath(Vector3 startPosition, Vector3 finalPosition, TeamEnum team)
    {
        List<Node> solution = new List<Node>();

        Node startNode = Grid.Instance.GetNode(startPosition);
        Node finalNode = Grid.Instance.GetNode(finalPosition);

        _openList = new List<Node>() { startNode };
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

        List<Vector3> path = GetVectorsPath(startNode, finalNode, team);
        ResetNodes();

        return path;
    }

    private List<Node> GetNodePath(Node node)
    {
        if (node.NodeParent == null)
            return new List<Node>() { node };
        else
        {
            List<Node> list = new List<Node>();
            list.AddRange(GetNodePath(node.NodeParent));
            list.Add(node);
            return list;
        }
    }

    private List<Vector3> GetVectorsPath(Node startNode, Node finalNode, TeamEnum team)
    {
        List<Node> nodePath = GetNodePath(finalNode);
        return GetVectorsPath(nodePath, startNode, finalNode, team);
    }

    private List<Vector3> GetVectorsPath(List<Node> nodePath, Node startNode, Node finalNode, TeamEnum team)
    {
        List<Vector3> path = new List<Vector3>();

        Entity topEntity;
        foreach (Node node in nodePath)
        {
            if (node == startNode)
                continue;

            topEntity = node.GetTopEntity();
            if (topEntity != null)
            {
                if (topEntity is Unit && topEntity.Team == team)
                    continue;

                if (node == finalNode && topEntity.Team != team)
                    continue;
            }

            path.Add(node.Position);
        }

        return path;
    }

    private void ResetNodes()
    {
        if (_openList == null || _closedList == null)
            return;
        if (_openList.Count == 0 && _closedList.Count == 0)
            return;

        List<Node> visitedNodes = new List<Node>();
        visitedNodes.AddRange(_openList);
        visitedNodes.AddRange(_closedList);

        foreach (Node node in visitedNodes)
        {
            node.GCost = int.MaxValue / 2;
            node.GCost = int.MaxValue / 2;
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
            if (topEntity != null && topEntity.Team != team)
                continue;
            
            neighbours.Add(node);
        }

        return neighbours;
    }
}
