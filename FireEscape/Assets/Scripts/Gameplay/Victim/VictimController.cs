using System.Collections;
using UnityEngine;

public class VictimController : MonoBehaviour, IInteractable, IHoverable
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _trampolineJumpForce;
    [SerializeField] private Rigidbody _hipsRigidbody;
    [SerializeField] private Rigidbody[] _disabledRigidbodies;
    [SerializeField] private float _minBurningTime = 10;
    [SerializeField] private float _maxBurningTime = 30f;
    [SerializeField] private float _groundCheckDistance;

    [Header("Sounds")]
    [SerializeField] private AudioClip _screamSounds;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _happySound;
    [SerializeField] private AudioClip _trampolineSound;

    private VictimCustomization _victimCustomization;
    private AudioSource _audioSource;
    private bool _isInWater;

    private void Awake()
    {
        _victimCustomization = GetComponent<VictimCustomization>();
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(TrampolineCheck());

        _audioSource.PlayOneShot(_screamSounds);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator TrampolineCheck()
    {
        while (true)
        {
            if (_hipsRigidbody.linearVelocity.sqrMagnitude < 0.01f)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            RaycastHit[] hits = Physics.RaycastAll(_hipsRigidbody.transform.position, Vector3.down, _groundCheckDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Trampoline"))
                {
                    _hipsRigidbody.AddForce(hit.collider.transform.up * Mathf.Abs(_hipsRigidbody.linearVelocity.y) * _trampolineJumpForce, ForceMode.Impulse);
                    _audioSource.PlayOneShot(_trampolineSound);
                    yield return new WaitForSeconds(.5f);
                    break;
                }

                if (hit.collider.TryGetComponent<WaterBuoyancy>(out var waterBuoyancy))
                {
                    _victimCustomization.SetActiveFloat(true);
                    _victimCustomization.SetHappyFace();

                    _isInWater = true;
                    StopBurning();

                    Destroy(this);
                }

                if (hit.collider.CompareTag("Ground") && _hipsRigidbody.linearVelocity.y < -10)
                {
                    StopBurning();
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void StartBurning()
    {
        float burningTime = Random.Range(_minBurningTime, _maxBurningTime);

        _victimCustomization.StartBurning(burningTime);
        StartCoroutine(Burning(burningTime));
    }

    private IEnumerator Burning(float burningTime)
    {
        yield return new WaitForSeconds(burningTime);

        StopBurning();
    }

    private void StopBurning()
    {
        if (!_isInWater)
        {
            foreach(var rb in _disabledRigidbodies)
            {
                rb.freezeRotation = false;
            }

            _victimCustomization.SetSadFace();
            _audioSource.PlayOneShot(_deathSound);
        }
        else
        {
            _audioSource.PlayOneShot(_happySound);
        }

        _victimCustomization.SetActiveBurning(false);
        
        Destroy(this);
    }

    public string GetInteractionPrompt()
    {
        return "Jump!";
    }

    public void Interact()
    {
        _hipsRigidbody.isKinematic = false;
        _hipsRigidbody.AddForce(transform.forward * _jumpForce, ForceMode.Impulse);
        Destroy(GetComponent<Collider>());
        _victimCustomization.SetActiveBurning(true);
        StartBurning();
        _audioSource.PlayOneShot(_screamSounds);
    }

    public void OnHoverEnter()
    {
        _victimCustomization.SetOutline(true);
    }

    public void OnHoverExit()
    {
        _victimCustomization.SetOutline(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(_hipsRigidbody.transform.position, _hipsRigidbody.transform.position + Vector3.down * _groundCheckDistance);
    }
}
