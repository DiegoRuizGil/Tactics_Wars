using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;

public class GridManagerTest
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
    public IEnumerator Positive_InitializeGrid()
    {
        GridManager manager = A.GridManager;
        yield return null;

        Assert.IsNotNull(Grid.Instance);
        Assert.AreEqual(10, Grid.Instance.Width);
        Assert.AreEqual(10, Grid.Instance.Height);
        Assert.AreEqual(4, Grid.Instance.GetNode(1, 1).Neighbours.Count);
    }
}
