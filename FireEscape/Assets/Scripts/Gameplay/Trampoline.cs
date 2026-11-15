using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(transform.up * rb.linearVelocity.y, ForceMode.Impulse);
        }
    }
}
