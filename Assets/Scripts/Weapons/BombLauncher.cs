using UnityEngine;
using System.Collections;
using PigiToolkit.Pooling;

public class BombLauncher : BaseGun
{
    [SerializeField] private Bomb _bombPrefab;

    protected override void OnStart()
    {
        MasterPooler.InitPool<Bomb>(_bombPrefab);
    }

    protected override void FireOnTarget(BaseDamagable target)
    {
        var bomb = MasterPooler.Get<Bomb>(transform.position, transform.rotation);
        bomb.LaunchBomb(this, 20);
    }
}
