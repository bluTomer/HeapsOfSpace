using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class BaseGun : PigiBehaviour
{
    [SerializeField] protected int gunDamage = 2;
    [SerializeField] protected float gunRange = 10.0f;
    [SerializeField] protected float shotDelay = 0.4f;
    [SerializeField] protected float firePrewarm = 0.0f;
    [SerializeField] protected float releaseCooldown = 0.1f;
    [SerializeField] protected bool oneShotOneAction = false;
    [SerializeField] protected LayerMask possibleTargets;

    protected bool isFiring;
    protected float nextShotTime;

    private bool _shotThisAction;
    private float _lastFireTime;

    public void Fire()
    {
        if (isFiring == false)
        {
            // Started firing
            isFiring = true;
            //nextShotTime = Time.time + firePrewarm;

            FireStarted();
        }

        // Set time to check for fire release
        _lastFireTime = Time.time;

        if (Time.time < nextShotTime)
        {
            // Still on cooldown
            return;
        }

        nextShotTime = Time.time + shotDelay;

        if (oneShotOneAction && _shotThisAction)
        {
            // Not firing anymore for this action
            return;
        }

        _shotThisAction = true;

        // Get target for shot
        var target = GetTarget();

        // Take shot on target
        FireOnTarget(target);
    }

    protected override void OnUpdate()
    {
        CheckFireRelease();
    }

    public virtual BaseDamagable GetTarget()
    {
        Debug.LogWarning("BaseGun GetTarget");
        // No targets for base gun
        return null;
    }

    protected virtual void FireOnTarget(BaseDamagable target)
    {
        Debug.LogWarning("BaseGun FireOnTarget");
    }

    protected virtual void FireStarted()
    {
        Debug.LogWarning("BaseGun FireStarted");
    }

    protected virtual void FireEnded()
    {
        Debug.LogWarning("BaseGun FireEnded");
    }

    private void CheckFireRelease()
    {
        if (!isFiring)
        {
            // Not firing, don't care
            return;
        }

        if (Time.time - _lastFireTime < releaseCooldown)
        {
            // Fire didn't stop yet
            return;
        }

        isFiring = false;
        _shotThisAction = false;

        FireEnded();
    }
}
