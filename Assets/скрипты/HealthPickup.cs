using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthPickup : MonoBehaviour
{
    public float healAmount = 25f;
    public AudioClip healSound;
    public ParticleSystem healEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(healAmount);

            if (healSound != null)
                AudioSource.PlayClipAtPoint(healSound, transform.position);

            if (healEffect != null)
                Instantiate(healEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
