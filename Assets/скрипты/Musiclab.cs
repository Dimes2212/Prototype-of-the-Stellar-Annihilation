using UnityEngine;
using System.Collections;

public class ZoneMusicController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip musicInsideZone; // Лаборатория
    public AudioClip musicOutsideZone; // Космос
    [Range(0.5f, 5f)] public float fadeDuration = 2f; // Длительность перехода

    [Header("Volume Control")]
    [Range(0f, 1f)] public float maxVolume = 1f; // Максимальная громкость
    [Range(0f, 1f)] public float insideZoneVolume = 1f; // Громкость в лаборатории
    [Range(0f, 1f)] public float outsideZoneVolume = 0.8f; // Громкость в космосе

    private bool isInsideZone;
    private Coroutine currentFade;

    void Start()
    {
        // Начинаем с музыки лаборатории
        audioSource.clip = musicInsideZone;
        audioSource.loop = true;
        audioSource.volume = insideZoneVolume * maxVolume;
        audioSource.Play();
        isInsideZone = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInsideZone)
        {
            isInsideZone = true;
            StartMusicTransition(musicInsideZone, insideZoneVolume * maxVolume,
                "Вошел в зону - плавно включаем музыку лаборатории");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isInsideZone)
        {
            isInsideZone = false;
            StartMusicTransition(musicOutsideZone, outsideZoneVolume * maxVolume,
                "Вышел из зоны - плавно включаем космическую музыку");
        }
    }

    void StartMusicTransition(AudioClip newClip, float targetVolume, string logMessage)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        Debug.Log(logMessage);
        currentFade = StartCoroutine(FadeMusic(newClip, targetVolume));
    }

    IEnumerator FadeMusic(AudioClip newClip, float targetVolume)
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
            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume; // Фиксируем конечную громкость
    }

    // Метод для ручной настройки громкости из других скриптов
    public void SetMaxVolume(float volume)
    {
        maxVolume = Mathf.Clamp01(volume);
        audioSource.volume = isInsideZone ?
            insideZoneVolume * maxVolume :
            outsideZoneVolume * maxVolume;
    }
}