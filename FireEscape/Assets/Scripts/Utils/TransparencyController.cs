using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private Material[] _defaultMaterials;
    private Material[] _transparentMaterials;

    public Material transparentBaseMaterial;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _defaultMaterials = _meshRenderer.materials;
        _transparentMaterials = new Material[_defaultMaterials.Length];

        for (int i = 0; i < _defaultMaterials.Length; i++)
        {
            Material newMaterial = new Material(transparentBaseMaterial);

            Color color = _defaultMaterials[i].color;
            color.a = 0.5f;

            newMaterial.color = color;

            _transparentMaterials[i] = newMaterial;
        }
    }

    public void SetTransparent(bool value)
    {
        _meshRenderer.materials = value
            ? _transparentMaterials
            : _defaultMaterials;
    }

    private void OnDestroy()
    {
        foreach (Material mat in _transparentMaterials)
        {
            Destroy(mat);
        }
    }
}