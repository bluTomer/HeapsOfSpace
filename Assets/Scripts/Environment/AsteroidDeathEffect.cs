using System;
using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class AsteroidDeathEffect : BaseBehaviour, IPoolable
{
    public Action<AsteroidDeathEffect> OnEffectEnd;

    public ParticleSystem PSystem { get; private set; }

    protected override void AssignComponents()
    {
        PSystem = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        PSystem.Play();
        StartCoroutine(EndEffect(PSystem.duration));
    }

    private IEnumerator EndEffect(float time)
    {
        yield return new WaitForSeconds(time);

        if (OnEffectEnd != null)
        {
            OnEffectEnd(this);
        }
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
