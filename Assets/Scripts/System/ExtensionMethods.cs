using UnityEngine;

public static class ExtensionMethods
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static void RotateTowards(this Transform tr, Vector2 dir, float speed)
    {
        var angleDelta = Vector3.Angle(dir, tr.up);
        var myDirection = tr.InverseTransformDirection(dir);

        if (myDirection.x > 0)
        {
            angleDelta *= -1;
        }
        tr.Rotate(Vector3.forward * angleDelta * speed);
    }
}