using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SceneInitializerTest
{
    [SetUp]
    public void SetUp()
    {
        int width = 24;
        int height = 17;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, new Vector3(-17, -10));
        Grid.Instance.SetNodesNeighbours();

        GameObject blueUnitsParent = new GameObject();
        GameObject blueBuildingsParent = new GameObject();

        GameObject redUnitsParent = new GameObject();
        GameObject redBuildingsParent = new GameObject();

        A.GameManager
            .WithUnitParent(TeamEnum.BLUE, blueUnitsParent.transform)
            .WithUnitParent(TeamEnum.RED, redUnitsParent.transform)
            .WithBuildingParent(TeamEnum.BLUE, blueBuildingsParent.transform)
            .WithBuildingParent(TeamEnum.RED, redBuildingsParent.transform);
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
    public IEnumerator InitializeSceneData_Positive()
    {
        string filePath = SaveSystem.NEW_GAME_SAVE_FOLDER + "DefaultSave.json";
        FileInfo saveFile = new FileInfo(filePath);

        SaveToLoadSO saveToLoadSO = A.SaveToLoad.WithFileInfo(saveFile).IsNewGame(true);
        EntitiesPrefabsSO entitesPrefabs = Resources.Load<EntitiesPrefabsSO>("ScriptableObjects/EntityInfo/EntitiesPrefabs");

        SceneInitializer initializer = A.SceneInitializer.WithSaveToLoadSO(saveToLoadSO).WithEntitiesPrefabs(entitesPrefabs);

        yield return null;

        int blueFoodResources = GameManager.Instance.FoodResources[TeamEnum.BLUE];
        int blueGoldResources = GameManager.Instance.GoldResources[TeamEnum.BLUE];

        int redFoodResources = GameManager.Instance.FoodResources[TeamEnum.RED];
        int redGoldResources = GameManager.Instance.GoldResources[TeamEnum.RED];

        initializer.InitializeSceneData();

        yield return null;

        Assert.AreEqual(2, GameManager.Instance.UnitLists[TeamEnum.BLUE].Count);
        Assert.AreEqual(1, GameManager.Instance.BuildingLists[TeamEnum.BLUE].Count);

        Assert.AreEqual(blueFoodResources, GameManager.Instance.FoodResources[TeamEnum.BLUE]);
        Assert.AreEqual(blueGoldResources, GameManager.Instance.GoldResources[TeamEnum.BLUE]);

        Assert.AreEqual(2, GameManager.Instance.UnitLists[TeamEnum.RED].Count);
        Assert.AreEqual(1, GameManager.Instance.BuildingLists[TeamEnum.RED].Count);

        Assert.AreEqual(redFoodResources, GameManager.Instance.FoodResources[TeamEnum.BLUE]);
        Assert.AreEqual(redGoldResources, GameManager.Instance.GoldResources[TeamEnum.BLUE]);

        Assert.AreEqual(1, GameManager.Instance.Turn);
    }
}
