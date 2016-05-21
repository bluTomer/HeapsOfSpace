using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class DualShockController : BaseBehaviour
{
    [SerializeField] private PlayerControls _playerShip;

    #region Axes

    public const string DS4_Lx = "DS4-Lx";
    public const string DS4_Ly = "DS4-Ly";

    public const string DS4_Rx = "DS4-Rx";
    public const string DS4_Ry = "DS4-Ry";

    #endregion

    #region Buttons

    public const string DS4_X = "DS4-X";
    public const string DS4_SQR = "DS4-Square";
    public const string DS4_CRCL = "DS4-Circle";
    public const string DS4_TRIG = "DS4-Triangle";
    public const string DS4_SHARE = "DS4-Share";
    public const string DS4_OPTN = "DS4-Options";
    public const string DS4_L1 = "DS4-L1";
    public const string DS4_L2 = "DS4-L2";
    public const string DS4_L3 = "DS4-L3";
    public const string DS4_R1 = "DS4-R1";
    public const string DS4_R2 = "DS4-R2";
    public const string DS4_R3 = "DS4-R3";
    public const string DS4_PS = "DS4-PS";
    public const string DS4_PAD = "DS4-Pad";

    #endregion

    protected override void OnUpdate()
    {
        ProcessMoveDirection();
        ProcessLookDirection();

        if (Pressed(DS4_R2))
        {
            _playerShip.FireBeam();
        }
        else if (Pressed(DS4_R1))
        {
            _playerShip.FireMissiles();
        }
    }

    private void ProcessMoveDirection()
    {
        var direction = new Vector2(Input.GetAxis(DS4_Lx), Input.GetAxis(DS4_Ly));
        if (direction.sqrMagnitude > 0)
        {
            _playerShip.Move(direction);
        }
    }

    private void ProcessLookDirection()
    {
        var direction = new Vector2(Input.GetAxis(DS4_Rx), Input.GetAxis(DS4_Ry));
        if (direction.sqrMagnitude > 0)
        {
            _playerShip.Look(direction);
        }
    }

    private bool Pressed(string button)
    {
        return Input.GetButton(button);
    }
}
