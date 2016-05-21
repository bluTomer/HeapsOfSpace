using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class Missile : BaseWarpable, IPoolable
{
    [SerializeField] private BaseDamagable _target;
    [SerializeField] private MissileExplodeEffect _effectPrefab;
    [SerializeField] private bool _launchOnStart;
    [SerializeField] private float _speed;
    [SerializeField] private float _turn;
    [SerializeField] private float _defaultMissileTimeout = 6.0f;
    [SerializeField] private bool _seeking;
    [SerializeField] private float _explosionRadius;

    private Rigidbody2D _rigidbody;

    private BaseGun _owner;
    private bool _isActive;
    private float _detonationTime;

    public void LaunchMissile(float missileTimeout)
    {
        _isActive = true;
        _detonationTime = Time.time + missileTimeout;
        Prethrust();
    }

    public void LaunchMissile(BaseGun owner, BaseDamagable target, float missileTimeout)
    {
        _target = target;
        _owner = owner;
        LaunchMissile(missileTimeout);
    }

    protected override void AssignComponents()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void OnStart()
    {
        MasterPooler.InitPool<MissileExplodeEffect>(_effectPrefab);
        if (_launchOnStart)
        {
            LaunchMissile(_defaultMissileTimeout);
        }
    }

    protected override void OnUpdate()
    {
        if (!_isActive)
        {
            return;
        }

        if (Time.time > _detonationTime)
        {
            Explode();
            return;
        }
       
        _rigidbody.AddForce(transform.up * _speed);

        // If target was destroyed or missing, go faulty
        if (_target == null)
        {
            // Try to find new target
            _target = _owner.GetTarget();

            // if target is still null, go faulty
            if (_target == null)
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * 300);
                return;
            }
        }

        if (_seeking)
        {
            var targetDirection = _target.transform.position - transform.position;

            transform.RotateTowards(targetDirection, _turn * Time.deltaTime);
        }
    }

    private void Prethrust()
    {
        _rigidbody.AddForce(transform.up * _speed * 30);
    }

    protected override void Explode()
    {
        _isActive = false;

        var targets = Physics2D.CircleCastAll(transform.position, _explosionRadius, transform.up);

        foreach (var hit in targets)
        {
            var damagable = hit.transform.GetComponent<BaseDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_owner.GunDamage);    
            }
        }

        var effect = MasterPooler.Get<MissileExplodeEffect>(transform.position, transform.rotation);
        effect.OnEffectEnd += OnEffectEnd;
        effect.Play();
        gameObject.SetActive(false);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Missile collided with " + collision.collider.gameObject.name);
        Explode();
    }

    private void OnEffectEnd(MissileExplodeEffect effect)
    {
        effect.OnEffectEnd -= OnEffectEnd;
        MasterPooler.Return<MissileExplodeEffect>(effect);
        MasterPooler.Return<Missile>(this);
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
