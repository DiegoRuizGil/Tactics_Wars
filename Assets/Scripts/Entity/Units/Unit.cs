using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Unit : Entity
{
    [Header("Unit Settings")]
    [SerializeField]
    private int _damage = 2;
    [SerializeField]
    private int _movementRange = 2;
    [SerializeField]
    private int _attackRange = 1;
    [SerializeField]
    private bool _hasMoved;
    [SerializeField]
    private bool _hasFinished;
    [SerializeField]
    private SpriteRenderer _sprite;
    [SerializeField]
    private Color _inactiveUnitColor;
    [SerializeField]
    private UnitType _unitType;
    [SerializeField]
    private List<UnitType> _weaknesses = new List<UnitType>();

    private Animator _animator;
    

    public int Damage { get { return _damage; } }
    public int MovementRange { get { return _movementRange; } }
    public int AttackRange { get { return _attackRange; } }
    public bool HasMoved { get { return _hasMoved; } set { _hasMoved = value; } }
    public bool HasFinished
    {
        get { return _hasFinished; }
        set {
            _hasFinished = value;
            if (_sprite == null)
                return;
            if (_hasFinished)
                _sprite.color = _inactiveUnitColor;
            else
                _sprite.color = Color.white;
        }
    }
    public UnitType UnitType { get { return _unitType; } set { _unitType = value; } }
    public List<UnitType> Weaknesses { get { return _weaknesses; } set { _weaknesses = value; } }
    public Animator Animator { get { return _animator; } }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void EntityDeath()
    {
        _animator.SetTrigger("Death");
    }

    public void DestroyObject() // called at the end of an animation
    {
        // Destroy(gameObject);
        GameManager.Instance.RemoveUnit(this);
    }
}

public enum UnitType
{
    NONE,
    ALDEANO,
    CABALLERO,
    CABALLERIA_LIGERA,
    ALABARDERO,
    PIQUERO,
    SOLDADO,
    MILICIA,
    BALLESTERO,
    ARQUERO
}