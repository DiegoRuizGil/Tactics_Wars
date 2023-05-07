using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AttackActionTest
{
    [TearDown]
    public void TearDown()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            GameObject.DestroyImmediate(obj);
        }
    }

    [UnityTest]
    public IEnumerator Positive_ApplyDamage()
    {
        Unit unit1 = An.Unit.WithUnitType(UnitType.ALABARDERO);
        Unit unit2 = An.Unit.WithUnitType(UnitType.CABALLERIA_LIGERA).WithWeaknesess(UnitType.ALABARDERO);

        yield return null;

        // unit1 -> unit2
        AttackAction action = new AttackAction(unit1, unit2);
        action.ApplyDamage();
        int damage = Mathf.RoundToInt(unit1.Damage * (1 + 0.5f));
        Assert.AreEqual(unit2.MaxHealth - damage, unit2.CurrentHealth);

        // unit2 -> unit1
        action = new AttackAction(unit2, unit1);
        action.ApplyDamage();
        damage = Mathf.RoundToInt(unit2.Damage * (1 - 0.5f));
        Assert.AreEqual(unit1.MaxHealth - damage, unit1.CurrentHealth);
    }
}
