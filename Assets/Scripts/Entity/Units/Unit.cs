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
    private bool _justInstantiated = false;
    [SerializeField]
    private SpriteRenderer _sprite;
    [SerializeField]
    private UnitType _unitType;
    [SerializeField]
    private List<UnitType> _weaknesses = new List<UnitType>();

    [SerializeField]
    private Animator _animator;
    private AnimationEventSystem _animEventSys;

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

            _sprite.material.SetFloat("_HasFinished", _hasFinished? 1f : 0f);

            if (_animator != null)
            {
                _animator.SetBool("Finished", _hasFinished);
            }
        }
    }
    public bool JustInstantiated { get { return _justInstantiated; } set { _justInstantiated = value; } }
    public UnitType UnitType { get { return _unitType; } set { _unitType = value; } }
    public List<UnitType> Weaknesses { get { return _weaknesses; } set { _weaknesses = value; } }
    public Animator Animator { get { return _animator; } }

    private void Start()
    {
        _animEventSys = GetComponent<AnimationEventSystem>();

        if (_entityDamagedEvent != null)
            _entityDamagedEvent?.Invoke(CurrentHealth * 1f / MaxHealth, false);
    }

    public override void EntityDeath()
    {
        _animator.SetTrigger("Death");
    }

    public void DestroyObject() // called at the end of an animation
    {
        GameManager.Instance.RemoveUnit(this);
        
        if (_animEventSys != null)
            _animEventSys.FinishAnimation();
    }

    public void SetAttackAnimation()
    {
        if (_animEventSys != null)
            _animator.SetTrigger("Attack");
    }

    public void SetHurtAnimation()
    {
        if (_animEventSys != null)
            _animator.SetTrigger("Hurt");
    }

    public void FlipSprite(Vector3 lookingAt)
    {
        if (_sprite == null)
            return;

        Vector3 unitPosition = new Vector3(
                Mathf.Floor(transform.position.x),
                Mathf.Floor(transform.position.y),
                transform.position.z
            );
        Vector3 targetPosition = new Vector3(
                Mathf.Floor(lookingAt.x),
                Mathf.Floor(lookingAt.y),
                lookingAt.z
            );

        Vector3 direction = (targetPosition - unitPosition).normalized;

        if (direction.x > 0)
            _sprite.flipX = false;
        else if (direction.x < 0)
            _sprite.flipX = true;
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