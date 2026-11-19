using UnityEngine;

public class TrampolineInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerHand;
    [SerializeField] private Material _transparentBaseMaterial;
    
    public string GetInteractionPrompt()
    {
        return "Take trampoline";
    }

    public void Interact()
    {
        transform.SetParent(_playerHand);
        transform.localPosition = Vector3.zero;

        GameManager.Instance.StartCatching();

        TransparencyController transparencyController = gameObject.AddComponent<TransparencyController>();
        transparencyController.transparentBaseMaterial = _transparentBaseMaterial;

        Destroy(this);
    }
}
