using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class KeyboardController : BaseBehaviour
{
    [SerializeField] private PlayerControls _playerShip;

    public const string MOVE_Y = "Keyboard-Y";
    public const string MOVE_X = "Keyboard-X";
    public const string ACTION_A = "Keyboard-ActionA";
    public const string ACTION_B = "Keyboard-ActionB";
    public const string ACTION_C = "Keyboard-ActionC";
    public const string ACTION_D = "Keyboard-ActionD";

    protected override void OnUpdate()
    {
        ProcessMoveDirection();
        ProcessLookDirection();
        ProcessActions();
    }

    private void ProcessMoveDirection()
    {
        var direction = new Vector2(Input.GetAxis(MOVE_X), Input.GetAxis(MOVE_Y));
        _playerShip.Move(direction);
    }

    private void ProcessLookDirection()
    {
        var mousePos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        var direction = worldPos - _playerShip.transform.position;
        _playerShip.Look(direction);
    }

    private void ProcessActions()
    {
        if (Input.GetButton(ACTION_A))
        {
            _playerShip.FireBeam();
        }
        else if (Input.GetButton((ACTION_C)))
        {
            _playerShip.FireMissiles();
        }
    }
}
