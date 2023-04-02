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

    private void Start()
    {
        if (TryGetComponent(out Building building))
        {
            _team = building.Team;
        }
    }

    public void GenerateResources(TeamEnum team)
    {
        if (_team == team)
            return;
        Debug.Log($"{_team}: Actualizando recursos");
        GameManager.Instance.UpdateResources(_team, _resourceType, _resourceAmount);
    }
}
