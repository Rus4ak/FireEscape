using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int _deathToLose = 10;
    [SerializeField] private TMP_Text _rescuedText;
    [SerializeField] private TMP_Text _lostText;
    [SerializeField] private GameObject _gameOverMenu;

    [HideInInspector] public int rescuedPeople;
    [HideInInspector] public int lostPeople;

    public static ScoreCounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _lostText.text = $"People Lost: {lostPeople} / {_deathToLose}";
    }

    public void AddRescuedPeople()
    {
        rescuedPeople++;

        _rescuedText.text = $"People Rescued: {rescuedPeople}";
    }

    public void AddLostPeople()
    {
        lostPeople++;

        _lostText.text = $"People Lost: {lostPeople} / {_deathToLose}";

        if (lostPeople >= _deathToLose)
        {
            _gameOverMenu.SetActive(true);
        }
    }
}
