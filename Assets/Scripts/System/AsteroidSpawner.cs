using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class AsteroidSpawner : BaseBehaviour
{
    public Asteroid AsteroidPrefab;
    public bool IsSpawning;
    public float SpawnInterval;
    public int MaxAsteroids;
    public Vector3[] SpawnPoints;
    public float[] Size;
    public int[] Health;
    public float[] Mass;

    private float _lastSpawnTime;

    protected override void OnStart()
    {
        MasterPooler.InitPool<Asteroid>(AsteroidPrefab);
    }

    protected override void OnUpdate()
    {
        if (!IsSpawning)
        {
            return;
        }

        if (Time.time > _lastSpawnTime + SpawnInterval && MasterPooler.ActiveObjectCount<Asteroid>() < MaxAsteroids)
        {
            var level = Random.Range(1, 3);
            var position = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            var fling = (Random.insideUnitCircle + (Vector2.one * 1.5f)) * Mass[level];

            SpawnAsteroid(level, position, fling);

            _lastSpawnTime = Time.time;
        }
    }

    public void SetSpawning(bool enabled)
    {
        IsSpawning = enabled;
    }

    public Asteroid SpawnAsteroid(int level, Vector3 position, Vector2 fling)
    {
        var asteroid = MasterPooler.Get<Asteroid>(position, Quaternion.identity);
        var asteroidRB = asteroid.GetComponent<Rigidbody2D>();
        asteroid.transform.localScale = Vector3.one * Size[level];
        asteroid.Level = level;
        asteroid.SetHitpoints(Health[level], Health[level]);
        asteroid.OnDeath += OnAsteroidDeath;
        asteroidRB.AddForce(fling, ForceMode2D.Impulse);
        asteroidRB.mass = Mass[level];

        return asteroid;
    }

    private void OnAsteroidDeath(Asteroid asteroid)
    {
        asteroid.OnDeath -= OnAsteroidDeath;

        Shake(asteroid.Level);

        if (asteroid.Level > 0)
        {
            var asteroidPos = new Vector2(asteroid.transform.position.x, asteroid.transform.position.y);
            var pos = Random.insideUnitCircle * asteroid.Level;
            var levelSqr = asteroid.Level * asteroid.Level;


            while (pos.sqrMagnitude < levelSqr * 0.5f)
            {
                pos = Random.insideUnitCircle * asteroid.Level;
            }

            var explosionDir = (asteroidPos + pos) - asteroidPos;
            SpawnAsteroid(asteroid.Level - 1, asteroidPos + pos, explosionDir * Mass[asteroid.Level - 1] * 2.0f);
            SpawnAsteroid(asteroid.Level - 1, asteroidPos - pos, explosionDir * Mass[asteroid.Level - 1] * -2.0f);
        }

        asteroid.gameObject.SetActive(false);

        MasterPooler.Return<Asteroid>(asteroid);
    }

    private void Shake(int level)
    {
        switch (level)
        {
            case 2:
                Context.Instance.CameraShake.Shake(0.3f, 6, 10);
                break;
            case 1:
                Context.Instance.CameraShake.Shake(0.2f, 4, 7);
                break;
            case 0:
                Context.Instance.CameraShake.Shake(0.1f, 4, 5);
                break;
            default:
                break;
        }
    }
}
