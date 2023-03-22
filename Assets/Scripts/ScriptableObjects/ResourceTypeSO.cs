using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewResourceType", menuName = "Resource Type")]
public class ResourceTypeSO : ScriptableObject
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private TileBase _resourceTile;

    public ResourceType ResourceType { get { return _resourceType; } set { _resourceType = value; } }
    public TileBase ResourceTile { get { return _resourceTile; } set { _resourceTile = value; } }
}
