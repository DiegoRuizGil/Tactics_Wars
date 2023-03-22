public class RepairAction : BaseAction
{
    private readonly Unit _selectedUnit;

    public RepairAction(Unit selectedUnit)
    {
        _selectedUnit = selectedUnit;
    }

    public override void Execute()
    {
        Entity building = Grid.Instance.GetNode(_selectedUnit.transform.position).GetEntity(0);

        if (building == null)
            return;

        building.RecoverHealth(building.MaxHealth / 2);

        _selectedUnit.HasFinished = true;
    }
}
