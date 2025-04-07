using UnityEngine;

public class SoundAndDestroy : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioSource targetAudioSource; // Ваш ручной AudioSource из сцены
    public AudioClip collisionSound;      // Звук столкновения

    [Header("Destruction")]
    public float destroyDelay = 0.5f;     // Задержка перед удалением

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlaySoundAndDestroy();
        }
    }

    void PlaySoundAndDestroy()
    {
        // 1. Проверяем компоненты
        if (targetAudioSource == null)
        {
            Debug.LogError("AudioSource не назначен!");
            Destroy(gameObject);
            return;
        }

        // 2. Воспроизводим звук (3 способа на выбор)
        if (collisionSound != null)
        {
            // Способ 1: Через PlayOneShot (рекомендуется)
            targetAudioSource.PlayOneShot(collisionSound);

            // Способ 2: Если нужно точно знать длину звука
            // targetAudioSource.clip = collisionSound;
            // targetAudioSource.Play();

            Debug.Log($"Playing sound: {collisionSound.name}");
        }
        else
        {
            Debug.LogWarning("Звук не назначен!");
        }

        // 3. Отключаем визуальную часть
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // 4. Удаляем с задержкой
        Destroy(gameObject, destroyDelay);
    }
}