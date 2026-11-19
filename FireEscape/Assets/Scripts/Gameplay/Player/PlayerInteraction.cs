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
    private IHoverable _currentHoverable;
    private TransparencyController _transparencyObject;

    private void Update()
    {
        _promptText.text = "";
        _currentInteractableObject = null;

        IHoverable newHoverable = null;

        RaycastHit[] hits = Physics.RaycastAll(_camera.position, _camera.forward, checkDistance);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool isFindTransparencyController = false;

        foreach (var hit in hits)
        {
            hit.collider.TryGetComponent(out newHoverable);

            if (_currentHoverable != newHoverable)
            {
                _currentHoverable?.OnHoverExit();

                _currentHoverable = newHoverable;

                _currentHoverable?.OnHoverEnter();
            }

            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                _promptText.text = "[E]\n" + interactable.GetInteractionPrompt();
                _currentInteractableObject = interactable;
            }

            else if (hit.collider.TryGetComponent<TransparencyController>(out var transparencyController))
            {
                transparencyController.SetTransparent(true);
                _transparencyObject = transparencyController;
                isFindTransparencyController = true;
            }

            else
            {
                break;
            }
        }

        if (_transparencyObject != null && !isFindTransparencyController)
        {
            _transparencyObject.SetTransparent(false);
            _transparencyObject = null;
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
