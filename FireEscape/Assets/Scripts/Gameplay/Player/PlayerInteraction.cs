using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private Transform _camera;

    public float checkDistance = 8f;

    private IInteractable _currentInteractableObject;

    private void Update()
    {
        _promptText.text = "";
        _currentInteractableObject = null;

        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, checkDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                _promptText.text = "[F]\n" + interactable.GetInteractionPrompt();
                _currentInteractableObject = interactable;
            }
        }
    }

    public void OnInteract(InputValue value)
    {
        if (_currentInteractableObject != null)
        {
            _currentInteractableObject.Interact();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(_camera.position, _camera.position + _camera.forward * checkDistance);
    }
}
