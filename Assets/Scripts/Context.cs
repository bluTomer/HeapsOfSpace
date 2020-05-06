using UnityEngine;
using System.Collections;
using PigiToolkit.Mono;

public class Context : Singleton<Context>
{
    [SerializeField] private int _playerNumber;
    [SerializeField] private Animator _uiAnimator;
    [SerializeField] private CameraControl _cameraShake;
    [SerializeField] private PlayerControls _playerPrefab;
    [SerializeField] private Vector2 _colliderPadding;
    [SerializeField] private LayerMask _colliderLayer;
    [SerializeField] private Material[] _playerMaterials;

    public int PlayerCount
    {
        get
        {
            var count = 0;

            for (int i = 0; i < _playersControllers.Length; i++)
            {
                if (_playersControllers[i] != null)
                {
                    count++;
                }
            }

            return count;
        }
    }

    private DualShockController[] _playersControllers;

    public CameraControl CameraShake { get { return _cameraShake; } }

    public const float TIMEOUT_TIME = 15;

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
        _playersControllers = new DualShockController[4];

        CreatePlayers(4);
    }

    protected override void OnUpdate()
    {
        // Kill idle players
        for (int i = 0; i < _playersControllers.Length; i++)
        {
            var player = _playersControllers[i];
            if (player != null && Time.time > player.LastInputTime + TIMEOUT_TIME)
            {
                Destroy(player.gameObject);
                _playersControllers[i] = null;
            }
        }

        // Add players on start pressed
        for (int i = 0; i < _playersControllers.Length; i++)
        {
            if (CheckForStart(i + 1))
            {
                _uiAnimator.SetTrigger("Show");
            }

            if (_playersControllers[i] == null && CheckForStart(i + 1))
            {
                CreatePlayer(i);
            }
        }
    }

    private bool CheckForStart(int playerID)
    {
        return Input.GetButton(string.Format(DualShockController.KEY_FORMAT, playerID, "START")) ||
        Input.GetButton(string.Format(DualShockController.KEY_FORMAT, playerID, "A"));
    }

    private void CreatePlayers(int count)
    {

        for (int i = 0; i < count; i++)
        {
            CreatePlayer(i);
        }
    }

    private void CreatePlayer(int i)
    {
        var player = Instantiate(_playerPrefab);
        player.transform.position = Vector3.zero;
        player.Setup(i + 1, _playerMaterials[i]);
        var dsController = player.gameObject.AddComponent<DualShockController>();
//        dsController._playerShip = player;
        dsController.PlayerID = i + 1;

        _playersControllers[i] = dsController;
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
