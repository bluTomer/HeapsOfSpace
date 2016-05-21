using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class Context : Singleton<Context>
{
    [SerializeField] private Vector2 _colliderPadding;
    [SerializeField] private LayerMask _colliderLayer;

    protected override void OnStart()
    {
        CreateCollider("Top", ScreenHelper.GetEdgePosition(Vector3.up * _colliderPadding.y),
            new Vector3(ScreenHelper.ScreenBounds.size.x * _colliderPadding.y, 1, 1), EdgeWrapper.EdgeType.Vertical);
        CreateCollider("Bottom", ScreenHelper.GetEdgePosition(Vector3.down * _colliderPadding.y),
            new Vector3(ScreenHelper.ScreenBounds.size.x * _colliderPadding.y, 1, 1), EdgeWrapper.EdgeType.Vertical);
        CreateCollider("Right", ScreenHelper.GetEdgePosition(Vector3.right * _colliderPadding.x),
            new Vector3(1, ScreenHelper.ScreenBounds.size.y * _colliderPadding.x, 1), EdgeWrapper.EdgeType.Horizontal);
        CreateCollider("Left", ScreenHelper.GetEdgePosition(Vector3.left * _colliderPadding.x), 
            new Vector3(1, ScreenHelper.ScreenBounds.size.y * _colliderPadding.x, 1), EdgeWrapper.EdgeType.Horizontal);
    }

    public GameObject CreateCollider(string name, Vector3 position, Vector3 size, EdgeWrapper.EdgeType edge)
    {
        var colliderObject = new GameObject(name);
        colliderObject.transform.position = position;
        colliderObject.transform.parent = transform;
        colliderObject.layer = 11;
        var collider = colliderObject.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = true;

        var edgeWrapper = colliderObject.AddComponent<EdgeWrapper>();
        edgeWrapper.Type = edge;

        return colliderObject;
    }
}
