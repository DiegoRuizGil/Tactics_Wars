using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent<float> _entityDamagedEvent;

    [Header("Entity Settings")]
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _maxHealth = 10;
    [SerializeField]
    private int _currentHealth = 10;
    [SerializeField]
    private TeamEnum _team;

    public string Name { get { return _name; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int CurrentHealth { get { return _currentHealth; } }
    public TeamEnum Team { get { return _team; } set { _team = value; } }

    public bool SetEntityInGrid()
    {
        Node node = Grid.Instance.GetNode(transform.position);
        if (node == null)
            return false;
        else
        {
            node.AddEntity(this);
            return true;
        }
    }

    public void ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        if (_currentHealth < damage)
            _currentHealth = 0;
        else
            _currentHealth -= damage;

        _entityDamagedEvent?.Invoke(_currentHealth * 1f / _maxHealth);
    }

    public void RecoverHealth(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        if (_currentHealth + amount > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += amount;

        _entityDamagedEvent?.Invoke(_currentHealth * 1f / _maxHealth);
    }

    public abstract void EntityDeath();
}

public enum TeamEnum
{
    BLUE = 0,
    RED = 1
}