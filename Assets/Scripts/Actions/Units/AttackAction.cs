using UnityEngine;

public class AttackAction : BaseAction
{
    private const float DAMAGE_COEFFICIENT = 0.25f;
    private readonly Unit _attacker;
    private readonly Entity _defender;

    public AttackAction(Unit attacker, Entity defender)
    {
        _attacker = attacker;
        _defender = defender;
        _isRunning = true;
    }

    public override void Execute()
    {
        AnimationEventSystem.AnimationFinishedEvent += ApplyDamage;

        _attacker.FlipSprite(_defender.transform.position);
        if (_defender is Unit)
            (_defender as Unit).FlipSprite(_attacker.transform.position);

        _attacker.SetAttackAnimation(); // action called at the end of the animation
    }

    public void ApplyDamage()
    {
        AnimationEventSystem.AnimationFinishedEvent -= ApplyDamage;

        bool isDead =_defender.ApplyDamage(CalculateDamage());

        if (_defender is Unit)
        {
            if (isDead)
            {
                SetActionFinished();
            }
            else
            {
                AnimationEventSystem.AnimationFinishedEvent += SetActionFinished;
                (_defender as Unit).SetHurtAnimation();
            } 
        }
        else if (_defender is Building)
        {
            SetActionFinished();
        }
    }

    public void SetActionFinished()
    {
        AnimationEventSystem.AnimationFinishedEvent -= SetActionFinished;

        _attacker.HasFinished = true;
        _isRunning = false;
        ActionFinished?.Invoke();
    }

    private int CalculateDamage()
    {
        if (_defender is not Unit)
            return _attacker.Damage;
        
        if ((_defender as Unit).Weaknesses.Contains(_attacker.UnitType))
        {
            return Mathf.RoundToInt(_attacker.Damage * (1 + DAMAGE_COEFFICIENT));
        }
        else if (_attacker.Weaknesses.Contains((_defender as Unit).UnitType))
        {
            return Mathf.RoundToInt(_attacker.Damage * (1 - DAMAGE_COEFFICIENT));
        }
        else
        {
            return _attacker.Damage;
        }  
    }
}
