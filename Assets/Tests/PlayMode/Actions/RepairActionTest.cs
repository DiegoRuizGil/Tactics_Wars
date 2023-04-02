using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RepairActionTest
{
    private Unit _unit;
    private Building _building;

    [SetUp]
    public void SetUp()
    {
        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);

        _unit = An.Unit.WithPosition(new Vector3(width - 1, height - 1, 0f));
        _building = An.Building.WithPosition(new Vector3(width - 1, height - 1, 0f));

        _building.ApplyDamage(_building.MaxHealth / 2);
    }

    [UnityTest]
    public IEnumerator Negative_Exectue()
    {
        RepairAction action = new RepairAction(_unit);
        action.Execute();

        Assert.AreEqual(_building.MaxHealth / 2, _building.CurrentHealth);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_Exectue()
    {
        Node node = Grid.Instance.GetNode(_unit.transform.position);
        node.AddEntity(_building);

        RepairAction action = new RepairAction(_unit);
        action.Execute();

        Assert.AreEqual(_building.MaxHealth, _building.CurrentHealth);

        yield return null;
    }
}
