using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundAndDestroy : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioSource targetAudioSource;
    public AudioClip collisionSound;
    public float destroyDelay = 0.5f;

    [Header("Key Settings")]
    public bool isKeyItem = false;
    [Tooltip("Общее количество ключей, необходимых для победы")]
    public static int totalKeysRequired = 4;
    public static int keysCollected = 0;

    [Header("UI Settings")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Text keysText;
    [SerializeField] private Text timeText;

    private static float gameStartTime;

    private void Start()
    {
        if (isKeyItem && keysCollected == 0)
        {
            gameStartTime = Time.time;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isKeyItem)
            {
                CollectKey();
            }
            PlaySoundAndDestroy();
        }
    }

    void CollectKey()
    {
        keysCollected++;
        Debug.Log($"Ключей собрано: {keysCollected}/{totalKeysRequired}");

        if (keysCollected >= totalKeysRequired)
        {
            ShowVictoryScreen();
        }
    }

    void PlaySoundAndDestroy()
    {
        if (targetAudioSource != null && collisionSound != null)
        {
            targetAudioSource.PlayOneShot(collisionSound);
        }

        // Отключаем рендер и коллайдер перед уничтожением
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null) renderer.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Destroy(gameObject, destroyDelay);
    }

    void ShowVictoryScreen()
    {
        if (victoryPanel == null) return;

        victoryPanel.SetActive(true);
        float playTime = Time.time - gameStartTime;
        timeText.text = $"Время: {Mathf.FloorToInt(playTime / 60)}:{Mathf.FloorToInt(playTime % 60):00}";
        keysText.text = $"Ключей собрано: {keysCollected}/{totalKeysRequired}";

        // Пауза игры
        Time.timeScale = 0f;
    }

    public static void ResetKeys()
    {
        keysCollected = 0;
    }
}