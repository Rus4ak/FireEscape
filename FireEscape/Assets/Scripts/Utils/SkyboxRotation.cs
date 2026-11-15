using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 1f;

    private void Update()
    {
        RenderSettings.skybox.SetFloat(
            "_Rotation",
            RenderSettings.skybox.GetFloat("_Rotation") +
            _rotationSpeed * Time.deltaTime
        );
    }
}