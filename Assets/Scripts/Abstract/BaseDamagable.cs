using UnityEngine;
using System;
using PigiToolkit.Mono;

public abstract class BaseDamagable : BaseBehaviour
{
    [SerializeField] protected bool _invincible;
    [SerializeField] protected int _hitpoints;
    [SerializeField] protected int _maxHP;

    private float _invincibleEndTime;

    public void SetHitpoints(int maxHP, int hitpoints)
    {
        if (hitpoints <= 0 || maxHP <= 0)
        {
            return;
        }

        _hitpoints = hitpoints;
        _maxHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (_invincible || damage < 0 || _hitpoints <= 0)
        {
            return;
        }

        _hitpoints -= damage;
        if (_hitpoints <= 0)
        {
            Die();
            return;
        }

        DamageEffect();
    }

    protected abstract void Die();

    public void SetInvincible(float time)
    {
        _invincibleEndTime = Time.time + time;
        _invincible = true;
    }

    protected override void OnUpdate()
    {
        if (_invincible && Time.time > _invincibleEndTime)
        {
            _invincible = false;
        }
    }

    public virtual void DamageEffect()
    {
    }
}
