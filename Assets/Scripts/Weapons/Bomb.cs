using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class Bomb : BaseWarpable, IPoolable
{
    [SerializeField] private MissileExplodeEffect _effectPrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _pulseInterval;
    [SerializeField] private float _damageRadius;

    private Rigidbody2D _rigidbody;

    private BaseGun _owner;
    private bool _isActive;
    private float _detonationTime;
    private float _nextPulseTime;

    public void LaunchBomb(BombLauncher owner, float bombTimeout)
    {
        _owner = owner;
        _isActive = true;
        _detonationTime = Time.time + bombTimeout;
        Prethrust();
    }

    protected override void AssignComponents()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void OnStart()
    {
    }

    private void Prethrust()
    {
        _rigidbody.AddForce(transform.up * _speed);
    }

    protected override void Explode()
    {
    }

    private void DamageOthers()
    {
        _isActive = false;

        var targets = Physics2D.CircleCastAll(transform.position, _damageRadius, transform.up);

        foreach (var hit in targets)
        {
            var damagable = hit.transform.GetComponent<BaseDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_owner.GunDamage);    
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EdgeWrapper>())
        {
            MasterPooler.Return<Bomb>(this);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time > _nextPulseTime)
        {
            DamageOthers();

            _nextPulseTime = Time.time + _pulseInterval;
        }
    }

    private void OnEffectEnd(MissileExplodeEffect effect)
    {
        effect.OnEffectEnd -= OnEffectEnd;
        MasterPooler.Return<MissileExplodeEffect>(effect);

    }

    #region IPoolable implementation

    public void Init()
    {
//        throw new System.NotImplementedException();
    }

    public void Reset()
    {
//        throw new System.NotImplementedException();
    }

    #endregion
}
