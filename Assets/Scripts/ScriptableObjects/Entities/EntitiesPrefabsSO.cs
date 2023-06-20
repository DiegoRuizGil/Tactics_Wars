using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesPrefabs", menuName = "Entities Prefabs")]
public class EntitiesPrefabsSO : ScriptableObject
{
    [SerializeField]
    private Unit[] _unitsPrefabs;
    [SerializeField]
    private Building[] _buildingsPrefabs;

    private Dictionary<string, Entity> _prefabs;

    [ContextMenu("Load Prefabs")]
    public void LoadPrefabs()
    {
        _prefabs = new Dictionary<string, Entity>();

        foreach(Unit unit in _unitsPrefabs)
            _prefabs[unit.Name] = unit;

        foreach (Building building in _buildingsPrefabs)
            _prefabs[building.Name] = building;
    }

    public bool TryGetPrefab(string name, out Entity prefab)
    {
        LoadPrefabs();

        if (_prefabs.TryGetValue(name, out Entity entity))
        {
            prefab = entity;
            return true;
        }

        prefab = null;
        return false;
    }
}
