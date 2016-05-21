using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class Rotate : BaseBehaviour
{
    [SerializeField] private Vector3 _rotationVector;

    protected override void OnUpdate()
    {
        transform.Rotate(_rotationVector * Time.deltaTime);
    }
}
