using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _trampolineHint;
    [SerializeField] private GameObject _trampolineControlHint;
    [SerializeField] private PlayerInteraction _playerInteraction;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartCatching()
    {
        _trampolineHint.SetActive(false);
        _trampolineControlHint.SetActive(true);
        _playerInteraction.checkDistance = 25f;
    }
}
