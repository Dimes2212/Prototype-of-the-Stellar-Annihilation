using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Text enemiesKilledText;
    [SerializeField] private Text timePlayedText;
    [SerializeField] private Text keysCollectedText;
    [SerializeField] private Button continueButton;

    [Header("Game Settings")]
    [SerializeField] private int totalKeysRequired = 4;

    private int enemiesKilled = 0;
    private int keysCollected = 0;
    private float gameStartTime;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameStartTime = Time.time;
        if (statsPanel != null) statsPanel.SetActive(false);

        // Настройка кнопки продолжения
        if (continueButton != null)
            continueButton.onClick.AddListener(ReturnToMenu);
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }

    public void KeyCollected()
    {
        keysCollected++;
        CheckGameCompletion();
    }

    private void CheckGameCompletion()
    {
        if (keysCollected >= totalKeysRequired)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        ShowStats();
    }

    private void ShowStats()
    {
        if (statsPanel == null) return;

        statsPanel.SetActive(true);

        float timePlayed = Time.time - gameStartTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timePlayed);

        enemiesKilledText.text = $"Enemies Killed: {enemiesKilled}";
        timePlayedText.text = $"Time Played: {timeSpan.Minutes}m {timeSpan.Seconds}s";
        keysCollectedText.text = $"Keys Collected: {keysCollected}/{totalKeysRequired}";

        // Пауза игры
        Time.timeScale = 0f;
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        // Здесь загружаем главное меню
        // SceneManager.LoadScene("MainMenu");
    }
}