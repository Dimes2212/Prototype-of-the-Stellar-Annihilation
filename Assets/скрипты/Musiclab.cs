using UnityEngine;
using System.Collections;

public class ZoneMusicController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip musicInsideZone; // Лаборатория
    public AudioClip musicOutsideZone; // Космос
    [Range(0.5f, 5f)] public float fadeDuration = 2f; // Длительность перехода

    private bool isInsideZone;
    private Coroutine currentFade;

    void Start()
    {
        // Начинаем с музыки лаборатории (по вашей версии)
        audioSource.clip = musicInsideZone;
        audioSource.loop = true;
        audioSource.volume = 1f;
        audioSource.Play();
        isInsideZone = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInsideZone)
        {
            isInsideZone = true;
            StartMusicTransition(musicInsideZone, "Вошел в зону - плавно включаем музыку лаборатории");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isInsideZone)
        {
            isInsideZone = false;
            StartMusicTransition(musicOutsideZone, "Вышел из зоны - плавно включаем космическую музыку");
        }
    }

    void StartMusicTransition(AudioClip newClip, string logMessage)
    {
        // Останавливаем предыдущий переход если был
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        Debug.Log(logMessage);
        currentFade = StartCoroutine(FadeMusic(newClip));
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        float timer = 0f;
        float startVolume = audioSource.volume;

        // Фаза 1: Плавное уменьшение текущей музыки
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Переключаем трек
        audioSource.clip = newClip;
        audioSource.Play();

        // Фаза 2: Плавное увеличение громкости нового трека
        timer = 0f;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f; // Фиксируем полную громкость
    }
}