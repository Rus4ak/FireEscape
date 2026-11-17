using UnityEngine;

public class VictimCustomization : MonoBehaviour
{
    [SerializeField] private GameObject[] _bodies;
    [SerializeField] private GameObject[] _hair;
    [SerializeField] private Material[] _scaredFaces;
    [SerializeField] private Material[] _happyFaces;
    [SerializeField] private Material[] _sadFaces;
    [SerializeField] private MeshRenderer _face;
    [SerializeField] private GameObject _float;

    private GameObject _activeBody;
    private GameObject _activeHair;

    private void Awake()
    {
        _activeBody = _bodies[Random.Range(0, _bodies.Length)];
        _activeBody.SetActive(true);
        _activeHair = _hair[Random.Range(0, _hair.Length)];
        _activeHair.SetActive(true);
        _face.material = _scaredFaces[Random.Range(0, _scaredFaces.Length)];
    }

    public void SetOutline(bool value)
    {
        if (value)
        {
            _activeBody.layer = LayerMask.NameToLayer("Outline");
            _activeHair.layer = LayerMask.NameToLayer("Outline");
        }
        else
        {
            _activeBody.layer = 0;
            _activeHair.layer = 0;
        }
    }
}
