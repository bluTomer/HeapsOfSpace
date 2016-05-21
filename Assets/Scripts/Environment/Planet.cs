using UnityEngine;
using System;
using PigiToolkit.Mono;

public class Planet : BaseBehaviour
{
    [SerializeField] private CircleCollider2D _planetCollider;
    [SerializeField] private Transform _model;

    public float PlanetRadius { get; private set; }

    private SpringJoint2D _spring;

    protected override void OnStart()
    {
        PlanetRadius = _model.localScale.x * 0.5f;
        _planetCollider.radius = PlanetRadius;
    }
}
