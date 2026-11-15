using UnityEngine;

public class TrampolineInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerHand;
    
    public string GetInteractionPrompt()
    {
        return "Take trampoline";
    }

    public void Interact()
    {
        transform.SetParent(_playerHand);
        transform.localPosition = Vector3.zero;

        GameManager.Instance.StartCatching();

        Destroy(this);
    }
}
