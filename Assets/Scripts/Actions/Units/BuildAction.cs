using UnityEngine;

public class BuildAction : BaseAction
{
    private readonly EntityInfoSO _buildingInfo;
    private readonly Unit _selectedUnit;
    private readonly GameObject _parent;

    public BuildAction(EntityInfoSO buildingInfo, Unit selectedUnit, GameObject parent)
    {
        _buildingInfo = buildingInfo;
        _selectedUnit = selectedUnit;
        _parent = parent;
    }

    public override void Execute()
    {
        Debug.Log($"Quitamos al jugador {_buildingInfo.FoodAmount} de comida y {_buildingInfo.GoldAmount} de oro.");
        Entity building = GameObject.Instantiate(
            _buildingInfo.EntityPrefab,
            _selectedUnit.transform.position,
            Quaternion.identity).GetComponent<Entity>();

        if (_parent != null)
            building.transform.parent = _parent.transform;

        if (building != null)
        {
            Node node = Grid.Instance.GetNode(_selectedUnit.transform.position);
            node.AddEntity(building);
        }

        _selectedUnit.HasFinished = true;
    }
}
