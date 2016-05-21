using UnityEngine;
using System.Collections;

public abstract class BaseWarpable : BaseDamagable
{
    public const float WARP_DAMPING = 0.9f;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (_invincible)
        {
            return;
        }

        HandleEdgeCollision(other);
    }

    private void HandleEdgeCollision(Collider2D other)
    {
        var edge = other.GetComponent<EdgeWrapper>();
        if (edge != null)
        {
            var pos = transform.position;
            var multi = edge.GetMultiplier();

            transform.position = new Vector3(pos.x * multi.x * WARP_DAMPING, pos.y * multi.y * WARP_DAMPING, pos.z * multi.z * WARP_DAMPING);
        }
    }
}
