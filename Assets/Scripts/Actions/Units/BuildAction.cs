using UnityEngine;

public class BuildAction : BaseAction
{
    private readonly BuildingInfoSO _buildingInfo;
    private readonly Unit _selectedUnit;
    private readonly TeamEnum _team;

    public BuildAction(BuildingInfoSO buildingInfo, Unit selectedUnit, TeamEnum team)
    {
        _buildingInfo = buildingInfo;
        _selectedUnit = selectedUnit;
        _team = team;
    }

    public override void Execute()
    {
        if (GameManager.Instance.UpdateResources(_team, _buildingInfo.FoodAmount * -1, _buildingInfo.GoldAmount * -1))
        {
            GameManager.Instance.InstantiateBuilding(
                    _buildingInfo.Entity,
                    _selectedUnit.transform.position,
                    _team
                );

            _selectedUnit.HasFinished = true;
        }
        else
        {
            Debug.LogWarning("No hay suficientes recursos para generar el edificio");
        }
    }
}
