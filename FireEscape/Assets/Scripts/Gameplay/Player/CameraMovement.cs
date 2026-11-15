using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private float _sensativity = 10f;

    private PlayerInput _playerInput;
    private InputAction _lookAction;
    private float _xRotation;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _lookAction = _playerInput.actions.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 movement = _lookAction.ReadValue<Vector2>() * _sensativity * Time.deltaTime;

        _xRotation -= movement.y;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);

        _head.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.Rotate(Vector3.up * movement.x);
    }
}
