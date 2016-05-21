using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class AsteroidSpawner : BaseBehaviour
{
    public Asteroid AsteroidPrefab;
    public float[] Size;
    public int[] Health;
    public float[] Mass;

    private Asteroid aster;

    protected override void OnStart()
    {
        MasterPooler.InitPool<Asteroid>(AsteroidPrefab);
    }

    public void Test()
    {
        SpawnAsteroid(2, Vector3.one, Vector2.left);
    }

    public void Testy()
    {
        aster.TakeDamage(20);
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

        aster = asteroid;

        return asteroid;
    }

    private void OnAsteroidDeath(Asteroid asteroid)
    {
        asteroid.OnDeath -= OnAsteroidDeath;

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
}
