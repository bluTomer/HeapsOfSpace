using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class Sun : BaseBehaviour
{
    [SerializeField] private Planet[] _planets;

    protected override void OnStart()
    {
        transform.position = ScreenHelper.GetEdgePosition(ScreenHelper.EdgeType.BottomLeft);
    }
}
