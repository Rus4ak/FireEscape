using System.Collections;
using UnityEngine;

public class VictimController : MonoBehaviour, IInteractable, IHoverable
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _trampolineJumpForce;
    [SerializeField] private Rigidbody _hipsRigidbody;
    [SerializeField] private float _groundCheckDistance;

    private VictimCustomization _victimCustomization;

    private void Awake()
    {
        _victimCustomization = GetComponent<VictimCustomization>();

        StartCoroutine(TrampolineCheck());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator TrampolineCheck()
    {
        while (true)
        {
            RaycastHit[] hits = Physics.RaycastAll(_hipsRigidbody.transform.position, Vector3.down, _groundCheckDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Trampoline"))
                {
                    _hipsRigidbody.AddForce(hit.collider.transform.up * Mathf.Abs(_hipsRigidbody.linearVelocity.y) * _trampolineJumpForce, ForceMode.Impulse);
                    yield return new WaitForSeconds(.5f);
                    break;
                }
            }

            yield return new WaitForFixedUpdate();
        }
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
