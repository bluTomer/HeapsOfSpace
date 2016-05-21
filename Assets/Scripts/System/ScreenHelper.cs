using UnityEngine;
using System.Collections;

public static class ScreenHelper
{
    public enum EdgeType
    {
        TopRight,
        TopCenter,
        TopLeft,
        MiddleRight,
        MiddleCenter,
        MiddleLeft,
        BottomRight,
        BottomCenter,
        BottomLeft
    }

    public enum OrientationType
    {
        Portrait,
        Landscape
    }

    public static Camera ScreenCamera { get; private set; }

    public static OrientationType ScreenOrientation { get; private set; }

    public static Bounds ScreenBounds { get; private set; }

    static ScreenHelper()
    {
        Reset(Camera.main);
    }

    public static void Reset(Camera camera)
    {
        ScreenCamera = camera;

        ScreenOrientation = (Screen.width > Screen.height) ? OrientationType.Landscape : OrientationType.Portrait;

        var extents = new Vector3(ScreenCamera.aspect, 1, 0) * ScreenCamera.orthographicSize;
        var size = extents * 2;

        ScreenBounds = new Bounds(Vector3.zero, size);
    }

    public static Vector3 GetEdgePosition(EdgeType type)
    {
        Vector3 result;

        switch (type)
        {
            case EdgeType.TopRight:
                result = GetEdgePosition(Vector3.up + Vector3.right);
                break;
            case EdgeType.TopCenter:
                result = GetEdgePosition(Vector3.up);
                break;
            case EdgeType.TopLeft:
                result = GetEdgePosition(Vector3.up + Vector3.left);
                break;
            case EdgeType.MiddleRight:
                result = GetEdgePosition(Vector3.right);
                break;
            case EdgeType.MiddleCenter:
                result = GetEdgePosition(Vector3.zero);
                break;
            case EdgeType.MiddleLeft:
                result = GetEdgePosition(Vector3.left);
                break;
            case EdgeType.BottomRight:
                result = GetEdgePosition(Vector3.down + Vector3.right);
                break;
            case EdgeType.BottomCenter:
                result = GetEdgePosition(Vector3.down);
                break;
            case EdgeType.BottomLeft:
                result = GetEdgePosition(Vector3.down + Vector3.left);
                break;
            default:
                result = Vector3.zero;
                break;
        }   

        return result;
    }

    public static Vector3 GetEdgePosition(Vector3 edge)
    {
        return new Vector3(edge.x * ScreenCamera.aspect, edge.y, edge.z) * ScreenCamera.orthographicSize;
    }
}
