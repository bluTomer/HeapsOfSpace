using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class Missile : BaseWarpable, IPoolable
{
    [SerializeField] private BaseDamagable _target;
    [SerializeField] private bool _launchOnStart;
    [SerializeField] private float _speed;
    [SerializeField] private float _turn;
    [SerializeField] private float _defaultMissileTimeout = 6.0f;
    [SerializeField] private bool _seeking;

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
            Die();
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

    protected override void Die()
    {
        Debug.Log("Missile Destroyed");
        _isActive = false;
        Destroy(gameObject);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
    }

    #region IPoolable implementation

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
