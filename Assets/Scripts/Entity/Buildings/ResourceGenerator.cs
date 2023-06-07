using UnityEngine;

[RequireComponent(typeof(Building))]
public class ResourceGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private ResourceType _resourceType;
    [SerializeField]
    private int _resourceAmount;

    public ResourceType ResourceType { get { return _resourceType; } }
    public int ResourceAmount { get { return _resourceAmount; } }

    private TeamEnum _team;

    private string _name;

    private void Start()
    {
        if (TryGetComponent(out Building building))
        {
            _team = building.Team;
            _name = building.Name;
        }
    }

    public void GenerateResources(TeamEnum team)
    {
        if (_team == team)
            return;
        Debug.Log($"[{_team}] {_name} ha generado: {_resourceAmount} de {_resourceType}");
        GameManager.Instance.UpdateResource(_team, _resourceType, _resourceAmount);
    }
}
