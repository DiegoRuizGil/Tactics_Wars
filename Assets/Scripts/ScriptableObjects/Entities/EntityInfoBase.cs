using UnityEngine;

public abstract class EntityInfoBase<T> : ScriptableObject where T : Entity
{
    [SerializeField] private T _entity;
    [SerializeField] private int _foodAmount;
    [SerializeField] private int _goldAmount;

    public T Entity { get { return _entity; } set { _entity = value; } }
    public int FoodAmount { get { return _foodAmount; } set { _foodAmount = value; } }
    public int GoldAmount { get { return _goldAmount; } set { _goldAmount = value; } }
}
