using UnityEngine;

public class GenerateUnitAction : BaseAction
{
    private readonly UnitInfoSO _unitInfo;
    private readonly Transform _parent;
    private readonly Vector3 _position;

    public GenerateUnitAction(UnitInfoSO unitInfo, Transform parent, Vector3 position)
    {
        _unitInfo = unitInfo;
        _parent = parent;
        _position = position;
    }

    public override void Execute()
    {
        Debug.Log($"Quitamos al jugador {_unitInfo.FoodAmount} de comida y {_unitInfo.GoldAmount} de oro.");

        Unit unit = GameObject.Instantiate(
            _unitInfo.Entity,
            _position,
            Quaternion.identity,
            _parent
        ).GetComponent<Unit>();

        unit.HasFinished = true;
        unit.ApplyDamage(unit.MaxHealth / 2);

        Node node = Grid.Instance.GetNode(_position);
        if (node != null)
            node.AddEntity(unit);
    }
}
