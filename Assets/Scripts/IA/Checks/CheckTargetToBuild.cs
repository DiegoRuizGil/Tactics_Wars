using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using BehaviourTree;

public class CheckTargetToBuild : TreeNode
{
    private Unit _unit;
    private Tilemap _resourcesTilemap;

    public CheckTargetToBuild(BehaviourTree.Tree tree, Unit unit, Tilemap resourcesTilemap)
        : base(tree)
    {
        _unit = unit;
        _resourcesTilemap = resourcesTilemap;
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
            List<Building> resourceBuildings = GameManager.Instance.BuildingLists[_unit.Team]
                .Where(b => b.gameObject.GetComponent<ResourceGenerator>() != null
                    && b.BuildingType != BuildingType.URBAN_CENTER)
                .ToList();

            List<Building> unitBuildings = GameManager.Instance.BuildingLists[_unit.Team]
                .Where(b => b.gameObject.GetComponent<UnitGenerator>() != null
                    && b.BuildingType != BuildingType.URBAN_CENTER)
                .ToList();

            


            return _state;
        }
        else
        {
            _state = TreeNodeState.SUCCESS;
            return _state;
        }
    }

    private void LookForResources()
    {

    }
}
