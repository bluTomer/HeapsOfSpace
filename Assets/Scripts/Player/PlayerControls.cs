using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class PlayerControls : BaseBehaviour
{
    [SerializeField] private LaserGun _laserGun;
    [SerializeField] private MissileLauncher _missileLauncher;
    [SerializeField] private float _thrustSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _blinkDistance;
    [SerializeField] private float _blinkCooldown;

    private float _lastBlinkTime;

    private SpringJoint2D _currentSpring;

    private Rigidbody2D _rb2D;

    protected override void AssignComponents()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        _rb2D.AddForce(direction * _thrustSpeed * Time.deltaTime);
    }

    public void Look(Vector2 direction)
    {
        transform.RotateTowards(direction, _turnSpeed * Time.deltaTime);
    }

    public void FireBeam()
    {
        _laserGun.Fire();
    }

    public void Blink(Vector2 direction)
    {
        if (Time.time < _lastBlinkTime + _blinkCooldown)
        {
            return;
        }

        transform.Translate(direction.normalized * _blinkDistance, Space.World);
        _lastBlinkTime = Time.time;
    }

    public void FireCannon()
    {
        //
    }

    public void FireMissiles()
    {
        _missileLauncher.Fire();
    }

    public void FireBomb()
    {
        Debug.Log("FireBomb");
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        HandleAsteroidCollision(other);
        HandleEdgeCollision(other);
    }

    private void HandleAsteroidCollision(Collider2D other)
    {
        if (other.attachedRigidbody == null || other.attachedRigidbody.GetComponent<Asteroid>() == null)
        {
            // Not an asteroid
            return;
        }

        DamagePlayer();
    }

    private void HandleEdgeCollision(Collider2D other)
    {
        var edge = other.GetComponent<EdgeWrapper>();
        if (edge == null)
        {
            return;
        }

        var pos = transform.position;
        var multi = edge.GetMultiplier();
        var warpDamping = 0.9f;

        transform.position = new Vector3(pos.x * multi.x * warpDamping, pos.y * multi.y * warpDamping, pos.z * multi.z * warpDamping);
    }

    private void DamagePlayer()
    {
        Debug.Log("Player Damaged");
    }
}
