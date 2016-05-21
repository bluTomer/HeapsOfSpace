﻿using UnityEngine;
using System.Collections;

public class MissileLauncher : BaseGun
{
    [SerializeField] private Missile _missilePrefab;

    public override BaseDamagable GetTarget()
    {
        BaseDamagable damagable = null;

        var closeObjects = Physics2D.CircleCastAll(transform.position, gunRange,
                               transform.up, gunRange, possibleTargets.value);
        foreach (var collider in closeObjects)
        {
            if (collider.rigidbody == null)
            {
                // Not a valid target
                continue;
            }

            damagable = collider.rigidbody.GetComponent<BaseDamagable>();

            if (damagable == null)
            {
                // Not a valid target
                continue;
            }

            // if we reached this, we have a valid target
            break;
        }

        // If no target was found, damagable will be null (that's ok)
        return damagable;
    }

    protected override void FireOnTarget(BaseDamagable target)
    {
        var missile = Instantiate<Missile>(_missilePrefab);
        missile.transform.position = transform.position;
        missile.transform.rotation = transform.rotation;

        if (target == null)
        {
            Debug.Log("Faulty");
            missile.LaunchMissile(this, null, 2);
            return;
        }

        missile.LaunchMissile(this, target, 6);
    }
}
