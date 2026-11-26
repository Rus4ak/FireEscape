using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextColor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _pressedColor;

    private Color _normalColor;

    private void Awake()
    {
        _normalColor = _text.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _text.color = _pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _text.color = _normalColor;
    }
}
