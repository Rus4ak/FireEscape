using System.Collections;
using UnityEngine;

public class VictimCustomization : MonoBehaviour
{
    [SerializeField] private GameObject[] _bodies;
    [SerializeField] private GameObject[] _hair;
    [SerializeField] private Material[] _scaredFaces;
    [SerializeField] private Material[] _happyFaces;
    [SerializeField] private Material[] _sadFaces;
    [SerializeField] private GameObject[] _fireParticles;
    [SerializeField] private MeshRenderer _face;
    [SerializeField] private GameObject _float;

    private GameObject _activeBody;
    private GameObject _activeHair;
    private GameObject _activeFire;
    private SkinnedMeshRenderer _bodyMeshRenderer;

    private void Awake()
    {
        _activeBody = _bodies[Random.Range(0, _bodies.Length)];
        _activeBody.SetActive(true);
        _activeHair = _hair[Random.Range(0, _hair.Length)];
        _activeHair.SetActive(true);
        _face.material = _scaredFaces[Random.Range(0, _scaredFaces.Length)];

        _bodyMeshRenderer = _activeBody.GetComponent<SkinnedMeshRenderer>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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

    public void SetActiveFloat(bool value)
    {
        _float.SetActive(value);
    }

    public void SetHappyFace()
    {
        _face.material = _happyFaces[Random.Range(0, _happyFaces.Length)];
    }

    public void SetSadFace()
    {
        _face.material = _sadFaces[Random.Range(0, _sadFaces.Length)];
    }

    public void StartBurning(float burningTime)
    {
        StartCoroutine(Burning(burningTime));
    }

    private IEnumerator Burning(float burningTime)
    {
        Material burningMaterial = new Material(_bodyMeshRenderer.material);
        Color startColor = burningMaterial.color;

        _bodyMeshRenderer.material = burningMaterial;

        float timer = 0;

        while (timer < burningTime)
        {
            timer += Time.deltaTime;
            burningMaterial.color = Color.Lerp(startColor, Color.black, timer / burningTime);

            yield return null;
        }
    }

    public void SetActiveBurning(bool value)
    {
        if (value)
        {
            _activeFire = _fireParticles[Random.Range(0, _fireParticles.Length)];
            _activeFire.SetActive(true);
        }
        else
        {
            _activeFire.SetActive(false);
            _activeFire = null;
            Destroy(this);
        }
    }
}
