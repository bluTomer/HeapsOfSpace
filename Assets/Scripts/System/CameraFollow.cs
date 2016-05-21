using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class CameraFollow : Singleton<CameraFollow>
{
    public static readonly Vector3 DEFAULT_CENTERED_DELTA = new Vector3(0, 0, -10);

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetDelta;
    [SerializeField] private float _zoomPadding = 1.5f;
    [SerializeField] private float _zoomDamping = 2.0f;
    [SerializeField] private float _followDamping = 2.0f;

    private Camera _camera;

    private float _targetSize;

    protected override void AssignComponents()
    {
        _camera = GetComponent<Camera>();
    }

    public void SetupTarget(Transform target)
    {
        _target = target;
        _targetDelta = transform.position - _target.position;
    }

    public void SetupTarget(Transform target, Vector3 delta)
    {
        _target = target;
        _targetDelta = delta;
    }

    public void SetZoom(float zoom)
    {
        _targetSize = zoom + _zoomPadding;
    }

    protected override void OnStart()
    {
        _targetDelta = transform.position - _target.position;
        _targetSize = _camera.orthographicSize;
    }

    protected override void OnFixedUpdate()
    {
        if (_target == null)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, _target.position + _targetDelta, Time.deltaTime * _followDamping);
    }

    protected override void OnUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (_camera.orthographicSize != _targetSize)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetSize, Time.deltaTime * _zoomDamping);
        }
    }
}
