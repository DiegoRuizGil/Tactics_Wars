using UnityEngine;

public class GenerateUnitAction : BaseAction
{
    private readonly UnitInfoSO _unitInfo;
    private readonly TeamEnum _team;
    private readonly Vector3 _position;

    public GenerateUnitAction(UnitInfoSO unitInfo, Vector3 position, TeamEnum team)
    {
        _unitInfo = unitInfo;
        _team = team;
        _position = position;
    }

    public override void Execute()
    {
        if (GameManager.Instance.UpdateResources(_team, _unitInfo.FoodAmount * -1, _unitInfo.GoldAmount * -1))
        {
            Unit unit = GameManager.Instance.InstantiateUnit(
                    _unitInfo.Entity,
                    _position,
                    _team
                );

            unit.HasFinished = true;
            unit.CurrentHealth = unit.MaxHealth / 2;
            unit.JustInstantiated = true;
        }
        else
        {
            Debug.LogWarning("No hay suficientes recursos para generar la unidad");
        }
    }
}
