
using System.Collections.Generic;
using UnityEngine;

public class WaterBuoyancy : MonoBehaviour
{
    [SerializeField] private float _buoyancyForce = 15f;
    [SerializeField] private float _waterDrag = 2f;
    [SerializeField] private float _submersionDepth = .5f;

    [Header("Waves")]
    [SerializeField] private float _waveHeight = .1f;
    [SerializeField] private float _waveSpeed = 1f;

    [Header("")]
    [SerializeField] private AudioSource _splashSound;

    private readonly Dictionary<Rigidbody, float> _defaultDrags = new();


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null || _defaultDrags.ContainsKey(rb))
            return;

        _defaultDrags.Add(rb, rb.linearDamping);

        _splashSound.transform.position = other.transform.position;
        _splashSound.volume = Mathf.Clamp01(Mathf.Abs(rb.linearVelocity.y) / 15);
        _splashSound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null || !_defaultDrags.ContainsKey(rb))
            return;

        rb.linearDamping = _defaultDrags[rb];
        _defaultDrags.Remove(rb);
    }

    private void FixedUpdate()
    {
        foreach (var pair in _defaultDrags)
        {
            Rigidbody rb = pair.Key;

            if (rb == null)
                continue;

            float wave = Mathf.Sin(Time.time * _waveSpeed + rb.GetInstanceID()) * _waveHeight;

            float targetHeight = transform.position.y -
                _submersionDepth + wave;

            float depth = targetHeight - rb.worldCenterOfMass.y;

            float force = Mathf.Clamp(
                depth * _buoyancyForce,
                -_buoyancyForce,
                _buoyancyForce
            );

            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);

            rb.linearDamping = _waterDrag;
        }
    }
}
