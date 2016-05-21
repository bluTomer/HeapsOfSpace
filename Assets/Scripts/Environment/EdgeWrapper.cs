using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class EdgeWrapper : PigiBehaviour
{
    public enum EdgeType
    {
        Vertical,
        Horizontal
    }

    public EdgeType Type;

    public Vector3 GetMultiplier()
    {
        if (Type == EdgeType.Vertical)
        {
            return new Vector3(1, -1, 1);
        }
        else
        {
            return new Vector3(-1, 1, 1);
        }
    }
}
