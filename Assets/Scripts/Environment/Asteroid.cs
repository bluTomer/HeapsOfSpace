using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PigiToolkit.Mono;
using PigiToolkit.Pooling;

public class Asteroid : BaseWarpable, IPoolable
{
    public int Level;

    public event Action<Asteroid> OnDeath;

    [SerializeField]
    private Image _healthBar;
    [SerializeField]
    private AsteroidDeathEffect _deathEffect;

    private Rigidbody2D _rigidBody;
    private Renderer _renderer;

    private Color _healthBarColor;

    protected override void AssignComponents()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    protected override void OnAwake()
    {
        _healthBarColor = _healthBar.color;
    }

    protected override void OnStart()
    {
        MasterPooler.InitPool<AsteroidDeathEffect>(_deathEffect);
    }

    protected override void Die()
    {
        var deathEffect = MasterPooler.Get<AsteroidDeathEffect>(transform.position, transform.rotation);
        deathEffect.transform.localScale = transform.localScale;
        deathEffect.OnEffectEnd += EndDeathEffect;
        deathEffect.Play();

        if (OnDeath != null)
        {
            OnDeath(this);
        }
    }

    public override void DamageEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRed());
    }

    private void EndDeathEffect(AsteroidDeathEffect effect)
    {
        effect.OnEffectEnd -= EndDeathEffect;
        MasterPooler.Return<AsteroidDeathEffect>(effect);
    }

    private IEnumerator FlashRed()
    {
        _healthBar.fillAmount = (float)_hitpoints / (float)_maxHP;
        _healthBar.color = _healthBarColor;

        _renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        _renderer.material.color = Color.white;

        yield return new WaitForSeconds(1.5f);

        _healthBar.color = Color.clear;
    }

    #region IPoolable implementation

    public void Init()
    {
        _healthBar.fillAmount = (float)_hitpoints / (float)_maxHP;
        _healthBar.color = Color.clear;
    }

    public void Reset()
    {
        StopAllCoroutines();
        _rigidBody.velocity = Vector2.zero;
        _renderer.material.color = Color.white;
    }

    #endregion
}
