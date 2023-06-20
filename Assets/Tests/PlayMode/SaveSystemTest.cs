using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SaveSystemTest
{
    private Unit _blueUnit, _redUnit;
    private Building _blueBuilding, _redBuilding;

    [SetUp]
    public void SetUp()
    {
        // Grid SetUp
        int width = 6;
        int height = 11;
        int cellSize = 1;
        Grid.Instance = new Grid(width, height, cellSize, Vector3.zero);
        Grid.Instance.SetNodesNeighbours();

        // GameManager SetUp
        GameObject blueUnitsParent = new GameObject();
        GameObject blueBuildingsParent = new GameObject();

        GameObject redUnitsParent = new GameObject();
        GameObject redBuildingsParent = new GameObject();

        A.GameManager
            .WithUnitParent(TeamEnum.BLUE, blueUnitsParent.transform)
            .WithUnitParent(TeamEnum.RED, redUnitsParent.transform)
            .WithBuildingParent(TeamEnum.BLUE, blueBuildingsParent.transform)
            .WithBuildingParent(TeamEnum.RED, redBuildingsParent.transform);

        // Entities SetUp
        _blueUnit = GameManager.Instance.InstantiateUnit(
            An.Unit.WithUnitType(UnitType.ARQUERO).WithName("Arquero"),
            Vector3.zero,
            TeamEnum.BLUE
        );
        _blueBuilding = GameManager.Instance.InstantiateBuilding(
            A.Building.WithType(BuildingType.URBAN_CENTER).WithName("Centro Urbano"),
            Vector3.up,
            TeamEnum.BLUE
        );

        _redUnit = GameManager.Instance.InstantiateUnit(
            An.Unit.WithUnitType(UnitType.ARQUERO).WithName("Alabardero"),
            Vector3.right,
            TeamEnum.RED
        );
        _redBuilding = GameManager.Instance.InstantiateBuilding(
            A.Building.WithType(BuildingType.URBAN_CENTER).WithName("Granja"),
            new Vector3(1,1),
            TeamEnum.RED
        );
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
    public IEnumerator Init_Positive()
    {
        SaveSystem.Init();

        Assert.IsTrue(Directory.Exists(SaveSystem.NEW_GAME_SAVE_FOLDER));
        Assert.IsTrue(Directory.Exists(SaveSystem.SAVE_FOLDER));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Save_Positive()
    {
        FileInfo newSave = SaveSystem.Save();
        string newSavePath = SaveSystem.SAVE_FOLDER + newSave.Name;
        string newSaveJSON = File.ReadAllText(newSavePath);

        Assert.IsTrue(File.Exists(newSavePath));

        _blueUnit.gameObject.transform.position = new Vector3(1, 1);
        _blueUnit.SetEntityInGrid();
        _blueBuilding.gameObject.transform.position = new Vector3(1,2);
        _blueBuilding.SetEntityInGrid();

        _redUnit.gameObject.transform.position = new Vector3(2,1);
        _redUnit.SetEntityInGrid();
        _redBuilding.gameObject.transform.position = new Vector3(2,2);
        _redBuilding.SetEntityInGrid();

        FileInfo modifiedSave = SaveSystem.Save(newSave.Name);
        string modifiedSavePath = SaveSystem.SAVE_FOLDER + modifiedSave.Name;
        string modifiedSaveJSON = File.ReadAllText(modifiedSavePath);

        Assert.IsTrue(File.Exists(modifiedSavePath));
        Assert.AreNotEqual(newSaveJSON, modifiedSaveJSON);

        File.Delete(SaveSystem.SAVE_FOLDER + modifiedSave.Name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator DeleteSaveFile_Positive()
    {
        string file = SaveSystem.Save().Name;

        Assert.IsTrue(SaveSystem.DeleteSaveFile(file));
        Assert.IsFalse(File.Exists(SaveSystem.SAVE_FOLDER + file));

        yield return null;
    }

    [UnityTest]
    public IEnumerator DeleteSaveFile_Negative()
    {
        string file = "ThisFileDontExist.json";

        Assert.IsFalse(SaveSystem.DeleteSaveFile(file));
        Assert.IsFalse(File.Exists(SaveSystem.SAVE_FOLDER + file));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Load_Positive()
    {
        FileInfo newSave = SaveSystem.Save();

        SceneData data = SaveSystem.Load(newSave.Name, false);

        List<Entity> entities = new List<Entity>();
        entities.AddRange(
            GameManager.Instance.UnitLists.Values.SelectMany(x => x).ToList()
        );
        entities.AddRange(
            GameManager.Instance.BuildingLists.Values.SelectMany(x => x).ToList()
        );

        foreach (EntityData entityData in data.entitiesData)
        {
            Entity entity = entities.FirstOrDefault(x => x.Name == entityData.name);
            Assert.AreEqual(entityData.team, entity.Team);
            Assert.AreEqual(entityData.position, entity.transform.position);
            Assert.AreEqual(entityData.currentHealth, entity.CurrentHealth);
        }

        foreach (ResourceData resourceData in data.resources)
        {
            Assert.AreEqual(GameManager.Instance.FoodResources[resourceData.team], resourceData.food);
            Assert.AreEqual(GameManager.Instance.GoldResources[resourceData.team], resourceData.gold);
        }

        Assert.AreEqual(data.gameData.turn, GameManager.Instance.Turn);

        File.Delete(SaveSystem.SAVE_FOLDER + newSave.Name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Load_Negative()
    {
        FileInfo newSave = SaveSystem.Save();

        File.WriteAllText(SaveSystem.SAVE_FOLDER + newSave.Name, "{\"json\"}");
        Assert.Throws<System.ArgumentException>(() => SaveSystem.Load(newSave.Name, false));

        Assert.IsNull(SaveSystem.Load("FILE.json", false));

        File.Delete(SaveSystem.SAVE_FOLDER + newSave.Name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator IsValidData_Positive()
    {
        FileInfo newSave = SaveSystem.Save();
        SceneData sceneData = SaveSystem.Load(newSave.Name, false);

        Assert.IsTrue(SaveSystem.IsValidData(sceneData));

        File.Delete(SaveSystem.SAVE_FOLDER + newSave.Name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator IsValidData_Negative()
    {
        FileInfo newSave = SaveSystem.Save();

        File.WriteAllText(SaveSystem.SAVE_FOLDER + newSave.Name, "{\"json\":\"json\"}");
        SceneData sceneData = SaveSystem.Load(newSave.Name, false);

        Assert.IsFalse(SaveSystem.IsValidData(sceneData));

        File.Delete(SaveSystem.SAVE_FOLDER + newSave.Name);

        yield return null;
    }
}
