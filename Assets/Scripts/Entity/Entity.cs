using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    protected UnityEvent<float, bool> _entityDamagedEvent;

    [Header("Entity Settings")]
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _maxHealth = 10;
    [SerializeField]
    private int _currentHealth = 10;
    [SerializeField]
    private TeamEnum _team;
    [SerializeField]
    private Sprite _entityImage;

    public string Name { get { return _name; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public TeamEnum Team { get { return _team; } set { _team = value; } }
    public Sprite EntityImage { get { return _entityImage; } }

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

    public bool ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        bool entityIsDead = _currentHealth <= damage;
        if (entityIsDead)
            _currentHealth = 0;
        else
            _currentHealth -= damage;
            
        _entityDamagedEvent?.Invoke(_currentHealth * 1f / _maxHealth, true);

        return entityIsDead;
    }

    public void RecoverHealth(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        if (_currentHealth + amount > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += amount;

        _entityDamagedEvent?.Invoke(_currentHealth * 1f / _maxHealth, true);
    }

    public abstract void EntityDeath(); // called in Healthbar events
}

public enum TeamEnum
{
    BLUE = 0,
    RED = 1
}