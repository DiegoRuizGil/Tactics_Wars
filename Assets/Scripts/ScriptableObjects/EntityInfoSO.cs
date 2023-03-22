using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityInfo", menuName = "Entity Info")]
public class EntityInfoSO : ScriptableObject
{
    [SerializeField] private GameObject _entityPrefab;
    [SerializeField] private int _foodAmount;
    [SerializeField] private int _goldAmount;

    public GameObject EntityPrefab { get { return _entityPrefab; } set { _entityPrefab = value; } }
    public int FoodAmount { get { return _foodAmount; } set { _foodAmount = value; } }
    public int GoldAmount { get { return _goldAmount; } set { _goldAmount = value; } }
}
