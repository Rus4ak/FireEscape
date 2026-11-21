using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = .3f;

    [Header("Hands")]
    [SerializeField] private Transform _hand;
    [SerializeField] private float _rotateDegrees;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private PlayerSounds _sounds;
    private Vector3 _movement;
    private InputAction _moveAction;
    private InputAction _sprintAction;
    private InputAction _rotateRightAction;
    private InputAction _rotateLeftAction;
    private bool _isOnGround;
    private bool _isSwimming;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _sounds = GetComponent<PlayerSounds>();

        _moveAction = _playerInput.actions.FindAction("Move");
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _rotateRightAction = _playerInput.actions.FindAction("RotateRight");
        _rotateLeftAction = _playerInput.actions.FindAction("RotateLeft");
    }

    private void Update()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();

        _movement = (transform.forward * input.y +
            transform.right * input.x).normalized;

        bool isSprint = false;

        if (_sprintAction.IsPressed())
        {
            _movement *= _sprintSpeed;
            isSprint = true;
        }
        else
            _movement *= _speed;

        if (_movement.sqrMagnitude < 0.1f || (!_isOnGround && !_isSwimming))
            _sounds.StopMovingSound();
        else
        {
            if (_isSwimming)
                _sounds.PlayMovingSound(MovingSoundType.Swimming, isSprint);
            else
                _sounds.PlayMovingSound(MovingSoundType.Walking, isSprint);
        }

        RotateHand();
    }

    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(_groundCheckPoint.position, _groundCheckRadius, Physics.DefaultRaycastLayers);

        _isOnGround = false;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                if (!hit.isTrigger)
                    _isOnGround = true;

                if (hit.CompareTag("Water"))
                    _isSwimming = true;
                else
                    _isSwimming = false;

                break;
            }
        }

        _rigidbody.linearVelocity = new Vector3(_movement.x,
            _rigidbody.linearVelocity.y, _movement.z);

        if (_isOnGround && _rigidbody.linearVelocity.y < -1)
            _sounds.PlayEndJumpingSound();
    }

    private void RotateHand()
    {
        Vector3 rotation = _hand.transform.rotation.eulerAngles;

        rotation.x = 0;
        rotation.z = 0;

        if (_rotateRightAction.IsPressed())
            rotation += Vector3.back * _rotateDegrees;
        else if (_rotateLeftAction.IsPressed())
            rotation += Vector3.forward * _rotateDegrees;

        _hand.rotation = Quaternion.Euler(rotation);
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && _isOnGround)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _sounds.PlayStartJumpingSound();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isOnGround ? Color.green : Color.red;

        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
