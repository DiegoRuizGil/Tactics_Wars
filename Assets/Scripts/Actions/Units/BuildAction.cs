using UnityEngine;

public class BuildAction : BaseAction
{
    private readonly BuildingInfoSO _buildingInfo;
    private readonly Unit _selectedUnit;
    private readonly Transform _parent;

    public BuildAction(BuildingInfoSO buildingInfo, Unit selectedUnit, Transform parent)
    {
        _buildingInfo = buildingInfo;
        _selectedUnit = selectedUnit;
        _parent = parent;
    }

    public override void Execute()
    {
        Debug.Log($"Quitamos al jugador {_buildingInfo.FoodAmount} de comida y {_buildingInfo.GoldAmount} de oro.");
        Transform tr = GameObject.Instantiate(
            _buildingInfo.Entity,
            _selectedUnit.transform.position,
            Quaternion.identity
        ).transform;

        if (_parent != null)
            tr.parent = _parent;

        Node node = Grid.Instance.GetNode(_selectedUnit.transform.position);
        node.AddEntity(_buildingInfo.Entity);

        _selectedUnit.HasFinished = true;
    }
}
