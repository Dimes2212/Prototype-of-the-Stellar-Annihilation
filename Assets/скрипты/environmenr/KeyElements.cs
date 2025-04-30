using UnityEngine;

public class SoundAndDestroy : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioSource targetAudioSource; 
    public AudioClip collisionSound;      

    [Header("Destruction")]
    public float destroyDelay = 0.5f;     

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlaySoundAndDestroy();
        }
    }

    void PlaySoundAndDestroy()
    {
        
        if (targetAudioSource == null)
        {
            Debug.LogError("AudioSource не назначен!");
            Destroy(gameObject);
            return;
        }

       
        if (collisionSound != null)
        {
            
            targetAudioSource.PlayOneShot(collisionSound);

            
            Debug.Log($"Playing sound: {collisionSound.name}");
        }
        else
        {
            Debug.LogWarning("Звук не назначен!");
        }

        
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

       
        Destroy(gameObject, destroyDelay);
    }
}