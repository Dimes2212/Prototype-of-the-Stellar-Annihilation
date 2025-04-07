using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;

public class KeyElements : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player"; 
    [SerializeField] private AudioClip disappearSound;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем тег столкнувшегося объекта
        if (collision.gameObject.CompareTag(targetTag))
        {
            if (disappearSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(disappearSound);
            }

            Destroy(gameObject); // Уничтожаем объект
            // Альтернатива: отключить видимость и коллайдер
            // GetComponent<Renderer>().enabled = false;
            // GetComponent<Collider>().enabled = false;
        }
    }
}