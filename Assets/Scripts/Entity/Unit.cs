using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    [Header("Unit Settings")]
    [SerializeField]
    private int _damage;
    [SerializeField]
    private int _movementRange;
    [SerializeField]
    private int _attackRange;
    [SerializeField]
    private bool _hasMoved = false;

    public int Damage { get { return _damage; } }
    public int MovementRange { get { return _movementRange; } }
    public int AttackRange { get { return _attackRange; } }
    public bool HasMoved { get { return _hasMoved; } set { _hasMoved = value; } }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }
}
