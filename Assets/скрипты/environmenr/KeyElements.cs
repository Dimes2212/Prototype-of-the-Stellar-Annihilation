using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollectibleItem : MonoBehaviour
{
    
    public AudioClip collectSound;
    public ParticleSystem collectEffect;

   

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collect();
            
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
        Destroy(gameObject);
    }
}