using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class DualShockController : BaseBehaviour
{
    public int PlayerID;

    private PlayerControls _playerShip;

    private const string KeyFormat = "GP{0}-DS4-{1}";

    public enum AxisType
    {
        Lx,
        Ly,
        Rx,
        Ry,
    }

    public enum ButtonType
    {
        X,
        Square,
        Circle,
        Triangle,
        L1,
        L2,
        L3,
        R1,
        R2,
        R3,
        Share,
        Options,
        PS,
        Pad,
    }

    private string GetKey(ButtonType button)
    {
        return string.Format(KeyFormat, PlayerID, button.ToString());
    }

    private string GetKey(AxisType axis)
    {
        return string.Format(KeyFormat, PlayerID, axis.ToString());
    }

    public Vector2 LeftStick
    {
        get
        {
            return new Vector2(Input.GetAxis(GetKey(AxisType.Lx)), Input.GetAxis(GetKey(AxisType.Ly)));
        }
    }

    public Vector2 RightStick
    {
        get
        {
            return new Vector2(Input.GetAxis(GetKey(AxisType.Rx)), Input.GetAxis(GetKey(AxisType.Ry)));
        }
    }

    protected override void AssignComponents()
    {
        _playerShip = GetComponent<PlayerControls>();
    }

    protected override void OnUpdate()
    {
        ProcessMoveDirection();
        ProcessLookDirection();

        if (Pressed(GetKey(ButtonType.R2)))
        {
            _playerShip.FireBeam();
        }
        else if (Pressed(GetKey(ButtonType.R1)))
        {
            _playerShip.FireMissiles();
        }
        else if (Pressed((GetKey(ButtonType.L1))))
        {
            _playerShip.FireBomb();
        }

        if (Pressed(GetKey(ButtonType.X)))
        {
            _playerShip.Blink(LeftStick.sqrMagnitude > 0 ? LeftStick : transform.up.ToVec2());
        }
    }

    private void Blink()
    {
        if (true)
        {
            
        }
    }

    private void ProcessMoveDirection()
    {
        if (LeftStick.sqrMagnitude > 0)
        {
            _playerShip.Move(LeftStick);
        }
    }

    private void ProcessLookDirection()
    {
        
        if (RightStick.sqrMagnitude > 0)
        {
            _playerShip.Look(RightStick);
        }
    }

    private bool Pressed(string button)
    {
        return Input.GetButton(button);
    }
}
