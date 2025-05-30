using UnityEngine;
using System.Collections;

public class ZoneMusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicInsideZone;  
    public AudioClip musicOutsideZone;  
    [Range(0.5f, 5f)] public float fadeDuration = 2f; 
    [Range(0f, 1f)] public float maxVolume = 1f;   
    [Range(0f, 1f)] public float insideZoneVolume = 1f;  
    [Range(0f, 1f)] public float outsideZoneVolume = 0.8f; 

    private bool isInsideZone;
    private Coroutine currentFade;

    void Start()
    {
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

        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        timer = 0f;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;    
    }

    public void SetMaxVolume(float volume)
    {
        maxVolume = Mathf.Clamp01(volume);
        audioSource.volume = isInsideZone ?
            insideZoneVolume * maxVolume :
            outsideZoneVolume * maxVolume;
    }
}