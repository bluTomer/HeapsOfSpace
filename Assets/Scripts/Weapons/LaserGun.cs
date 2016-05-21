using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LaserGun : BaseGun
{
    private LineRenderer _lineRenderer;
    private float _targetDistance;

    protected override void AssignComponents()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void OnUpdate()
    {
        // Don't replace BaseGuns' Update
        base.OnUpdate();

        if (isFiring)
        {
            // Get target is called to update the distance for the effect
            GetTarget();
            ShowEffect(_targetDistance);
        }
    }

    protected override void FireStarted()
    {
        _lineRenderer.enabled = true;
    }

    public override BaseDamagable GetTarget()
    {
        var ray = new Ray2D(transform.position, transform.up);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, gunRange, possibleTargets.value);

        if (hit.rigidbody == null)
        {
            // no target in range
            _targetDistance = gunRange;
            return null;
        }

        // Used to determine laser length
        _targetDistance = Vector3.Distance(transform.position, hit.point);

        var damagable = hit.rigidbody.GetComponent<BaseDamagable>();

        // If target is not damagable null will be returned
        return damagable;
    }

    protected override void FireOnTarget(BaseDamagable target)
    {
        if (target != null)
        {
            target.TakeDamage(gunDamage);
        }
    }

    protected override void FireEnded()
    {
        _lineRenderer.enabled = false;
    }

    private void ShowEffect(float length)
    {
        _lineRenderer.SetVertexCount(2);
        _lineRenderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.up * length });
    }
}
