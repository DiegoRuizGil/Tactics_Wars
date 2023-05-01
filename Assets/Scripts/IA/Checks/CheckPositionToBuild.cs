using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviourTree;

public class CheckPositionToBuild : TreeNode
{
    private Unit _unit;

    public CheckPositionToBuild(BehaviourTree.Tree tree, Unit unit)
        : base(tree)
    {
        _unit = unit;
    }

    public override TreeNodeState Evaluate()
    {
        if (_unit.HasMoved)
        {
            _state = TreeNodeState.FAILURE;
            return _state;
        }

        Vector3? posNullable = Tree.GetData("targetPosition") as Vector3?;
        if (posNullable == null)
        {
            ResourceType buildMode = (Tree.GetData("buildMode") as ResourceType?).Value;

            if (buildMode == ResourceType.NONE)
            {
                Building urbanCenter = GameManager.Instance.BuildingLists[_unit.Team]
                    .Where(building => building.BuildingType == BuildingType.URBAN_CENTER).FirstOrDefault();

                if (urbanCenter == null)
                {
                    _state = TreeNodeState.FAILURE;
                    return _state;
                }

                Node nodeUrbanCenter = Grid.Instance.GetNode(urbanCenter.transform.position);

                foreach (Node neighbour in nodeUrbanCenter.Neighbours)
                {
                    if (neighbour.GetEntity(0) == null)
                    {
                        Tree.SetData("targetPosition", neighbour.Position);

                        _state = TreeNodeState.SUCCESS;
                        return _state;
                    }
                }

                _state = TreeNodeState.FAILURE;
                return _state;
            }
            else
            {
                List<Node> resourcesNodes = Grid.Instance.GetNodesWithResourceType(buildMode);
                resourcesNodes.Sort((n1, n2) =>
                {
                    int dist1 = AStarPathfinding.Instance.GetPath(_unit.transform.position, n1.Position, _unit.Team).Count;
                    int dist2 = AStarPathfinding.Instance.GetPath(_unit.transform.position, n2.Position, _unit.Team).Count;

                    return dist1.CompareTo(dist2);
                });

                foreach (Node node in resourcesNodes)
                {
                    // FARMS
                    if (buildMode == ResourceType.FOOD && node.GetEntity(0) != null && node.GetEntity(0).Team == _unit.Team)
                    {
                        foreach (Node neighbour in node.Neighbours)
                        {
                            if (neighbour.GetEntity(0) == null)
                            {
                                Tree.SetData("targetPosition", neighbour.Position);

                                _state = TreeNodeState.SUCCESS;
                                return _state;
                            }
                        }
                    }
                    
                    // WINDMILL AND GOLDMINES
                    if (node.GetEntity(0) == null)
                    {
                        Tree.SetData("targetPosition", node.Position);

                        _state = TreeNodeState.SUCCESS;
                        return _state;
                    }
                }

                _state = TreeNodeState.FAILURE;
                return _state;
            }
        }
        else
        {
            _state = TreeNodeState.SUCCESS;
            return _state;
        }
    }
}
