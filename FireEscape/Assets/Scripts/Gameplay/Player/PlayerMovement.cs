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
    private Vector3 _movement;
    private InputAction _moveAction;
    private InputAction _sprintAction;
    private InputAction _rotateRightAction;
    private InputAction _rotateLeftAction;
    private bool _isOnGround;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

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

        if (_sprintAction.IsPressed())
            _movement *= _sprintSpeed;
        else
            _movement *= _speed;

        RotateHand();
    }

    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(_groundCheckPoint.position, _groundCheckRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        _isOnGround = false;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                _isOnGround = true;
                break;
            }
        }

        _rigidbody.linearVelocity = new Vector3(_movement.x,
            _rigidbody.linearVelocity.y, _movement.z);
    }

    private void RotateHand()
    {
        if (_rotateRightAction.IsPressed())
            _hand.localRotation = Quaternion.Euler(Vector3.back * _rotateDegrees);
        else if (_rotateLeftAction.IsPressed())
            _hand.localRotation = Quaternion.Euler(Vector3.forward * _rotateDegrees);
        else
            _hand.localRotation = Quaternion.identity;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && _isOnGround)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isOnGround ? Color.green : Color.red;

        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
