using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

#region JSON classes
[System.Serializable]
public class SceneData
{
    public EntityData[] entitiesData;
    public ResourceData[] resources;
    public GameData gameData;
}

[System.Serializable]
public class EntityData
{
    public string name;
    public TeamEnum team;
    public int currentHealth;
    public bool hasMoved;
    public bool hasFinished;
    public bool justInstantiated;
    public Vector3 position;
}

[System.Serializable]
public class ResourceData
{
    public TeamEnum team;
    public int food;
    public int gold;
}

[System.Serializable]
public class GameData
{
    public int turn;
}
#endregion


public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Resources/Saves/";
    public static readonly string NEW_GAME_SAVE_FOLDER = Application.dataPath + "/Resources/Saves/NewGame/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        
        if (!Directory.Exists(NEW_GAME_SAVE_FOLDER))
        {
            Directory.CreateDirectory(NEW_GAME_SAVE_FOLDER);
        }
    }

    public static FileInfo Save() // create new save
    {
        return Save(null);
    }

    public static FileInfo Save(string saveFile)
    {
        if (saveFile == null)
        {
            saveFile = GetNewFileName() + ".json";
        }

        string savePath = SAVE_FOLDER + saveFile;

        string saveContent = GetSceneDataString();

        File.WriteAllText(savePath, saveContent);

        return new FileInfo(savePath);
    }

    public static SceneData Load(string saveFileName, bool isNewGame)
    {
        string filePath = "";

        if (isNewGame)
        {
            filePath = NEW_GAME_SAVE_FOLDER + saveFileName;
        }
        else
        {
            filePath = SAVE_FOLDER + saveFileName;
        }

        if (File.Exists(filePath))
        {
            string saveString = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SceneData>(saveString);
        }
        else
        {
            Debug.Log($"[LOAD] Ruta no encontrada: {filePath}");
            return null;
        }
    }

    public static bool DeleteSaveFile(string saveFile)
    {
        string filePath = SAVE_FOLDER + saveFile;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        else
        {
            return false;
        }
    }

    private static string GetSceneDataString()
    {
        string data = "";

        List<EntityData> entitiesData = new List<EntityData>();
        foreach (Entity unit in GameManager.Instance.UnitLists.Values.SelectMany(x => x).ToList())
        {
            entitiesData.Add(
                new EntityData
                {
                    name = unit.Name,
                    team = unit.Team,
                    currentHealth = unit.CurrentHealth,
                    hasMoved = (unit as Unit).HasMoved,
                    hasFinished = (unit as Unit).HasFinished,
                    justInstantiated = (unit as Unit).JustInstantiated,
                    position = unit.transform.position
                }
            );
        }
        foreach (Entity building in GameManager.Instance.BuildingLists.Values.SelectMany(x => x).ToList())
        {
            entitiesData.Add(
                new EntityData
                {
                    name = building.Name,
                    team = building.Team,
                    currentHealth = building.CurrentHealth,
                    hasMoved = false,
                    hasFinished = false,
                    justInstantiated = false,
                    position = building.transform.position
                }
            );
        }

        List<ResourceData> resources = new List<ResourceData>();
        foreach (TeamEnum team in GameManager.Instance.FoodResources.Keys)
        {
            resources.Add(
                new ResourceData
                {
                    team = team,
                    food = GameManager.Instance.FoodResources[team],
                    gold = GameManager.Instance.GoldResources[team]
                }
            );
        }

        GameData gameData = new GameData
        {
            turn = GameManager.Instance.Turn
        };

        SceneData sceneData = new SceneData
        {
            entitiesData = entitiesData.ToArray(),
            resources = resources.ToArray(),
            gameData = gameData
        };

        data = JsonUtility.ToJson(sceneData);

        return data;
    }

    private static string GetNewFileName()
    {
        DateTime now = DateTime.Now;
        string fileName = "";

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(now.ToString());
            byte[] hashBytes = sha256.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            fileName = builder.ToString();
        }

        return fileName;
    }

}
