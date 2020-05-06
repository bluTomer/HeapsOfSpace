using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class DualShockController : BaseBehaviour
{
    public int PlayerID;

    private PlayerControls _playerShip;

    public const string KEY_FORMAT = "GP{0}-{1}";

    public float LastInputTime { get; private set; }

    public enum AxisType
    {
        Lx,
        Ly,
        Rx,
        Ry,
        R2L2,
    }

    public enum ButtonType
    {
        B,
        A,
        X,
        Y,
        L1,
        L3,
        R1,
        R3,
        START,
        BACK,
        Share,
        Options,
        PS,
        Pad,
    }

    public string GetKey(ButtonType button)
    {
        return string.Format(KEY_FORMAT, PlayerID, button.ToString());
    }

    public string GetKey(AxisType axis)
    {
        return string.Format(KEY_FORMAT, PlayerID, axis.ToString());
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

    protected override void OnStart()
    {
        LastInputTime = Time.time;
    }

    protected override void OnUpdate()
    {
        ProcessMoveDirection();
        ProcessLookDirection();

        if (Pressed(GetKey(ButtonType.R1)))
        {
            LastInputTime = Time.time;
            _playerShip.FireBeam();
        }
        else if (Pressed(GetKey(ButtonType.L1)))
        {
            LastInputTime = Time.time;
            _playerShip.FireMissiles();
        }
        else if (Mathf.Abs(Input.GetAxisRaw(GetKey(AxisType.R2L2))) > 0.5f)
        {
            LastInputTime = Time.time;
            _playerShip.FireBomb();
        }

        if (Pressed(GetKey(ButtonType.Y)))
        {
            _playerShip.Blink(LeftStick.sqrMagnitude > 0 ? LeftStick : transform.up.ToVec2());
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

    public bool Pressed(string button)
    {
        return Input.GetButton(button);
    }
}
