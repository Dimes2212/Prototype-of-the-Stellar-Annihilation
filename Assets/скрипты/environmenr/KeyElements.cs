using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollectibleItem : MonoBehaviour
{
    [Header("Effects")]
    public AudioClip collectSound;
    public ParticleSystem collectEffect;

    [Header("Collision")]
    public float minCollisionForce = 1f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float force = collision.relativeVelocity.magnitude;
            if (force >= minCollisionForce)
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        GameManager.Instance?.AddKey();

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 0.5f);
    }
}