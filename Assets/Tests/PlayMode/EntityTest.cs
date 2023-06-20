using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EntityTest
{
    [SetUp]
    public void SetUp()
    {
        Grid.Instance = new Grid(6, 11, 1, Vector3.zero);
    }

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
    public IEnumerator Negative_ApplyDamage()
    {
        GameObject gameObject = new GameObject();
        Entity unit = gameObject.AddComponent<Unit>();

        Assert.Throws<ArgumentOutOfRangeException>(() => unit.ApplyDamage(-1));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Negative_RecoverHealth()
    {
        GameObject gameObject = new GameObject();
        Entity unit = gameObject.AddComponent<Unit>();

        Assert.Throws<ArgumentOutOfRangeException>(() => unit.RecoverHealth(-1));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Negative_SetEntityInGrid()
    {
        GameObject gameObject = new GameObject();
        Entity unit = gameObject.AddComponent<Unit>();
        unit.transform.position = new Vector3(-1f, -1f, 0f);

        Assert.IsFalse(unit.SetEntityInGrid());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_ApplyDamage()
    {
        GameObject gameObject = new GameObject();
        Entity unit = gameObject.AddComponent<Unit>();
        int damage = unit.MaxHealth / 2;

        unit.ApplyDamage(damage);
        Assert.AreEqual(unit.MaxHealth - damage, unit.CurrentHealth);

        unit.ApplyDamage(unit.MaxHealth);
        Assert.AreEqual(0, unit.CurrentHealth);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_RecoverHealth()
    {
        GameObject gameObject = new GameObject();
        Entity unit = gameObject.AddComponent<Unit>();
        int damage = unit.MaxHealth;
        unit.ApplyDamage(damage);

        unit.RecoverHealth(damage / 2);
        Assert.Greater(damage, unit.CurrentHealth);

        unit.RecoverHealth(unit.MaxHealth);
        Assert.AreEqual(unit.MaxHealth, unit.CurrentHealth);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Positive_SetEntityInGrid()
    {
        GameObject gameObject = new GameObject();
        Entity unit = gameObject.AddComponent<Unit>();

        Assert.IsTrue(unit.SetEntityInGrid());
        Assert.AreEqual(unit, Grid.Instance.GetNode(0, 0).GetTopEntity());

        yield return null;
    }
}
