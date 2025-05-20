using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings")]
    public int keysToWin = 4;

    [Header("Stats")]
    public int keysCollected { get; private set; }
    public int enemiesKilled { get; private set; }
    public float playTime { get; private set; }

    [Header("UI")]
    public GameObject statsPanel;
    public TextMeshProUGUI keysText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timeText;

    private bool isGameActive = true;
    private bool hasWon = false; // ← добавляем флаг победы

    private void Awake()
    {
        Time.timeScale = 1f;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
        }
    }

    private void Update()
    {
        if (isGameActive)
        {
            playTime += Time.deltaTime;
        }
    }

    public void AddKey()
    {
        keysCollected++;
        Debug.Log($"Ключей собрано: {keysCollected}");
        CheckWinCondition();
    }


    public void AddKill()
    {
        enemiesKilled++;
    }

    private void CheckWinCondition()
    {
        if (hasWon) return; // ← предотвращаем повторный вызов

        if (keysCollected >= keysToWin)
        {
            hasWon = true;           // ← ставим флаг победы
            ShowStats();
            // Time.timeScale = 0f;  ← УБРАНО! Игра НЕ ставится на паузу
        }
    }

    private void ShowStats()
    {
        statsPanel.SetActive(true);
        keysText.text = $"KEYS: {keysCollected}/{keysToWin}";
        killsText.text = $"KILLS: {enemiesKilled}";
        timeText.text = $"TIME: {Mathf.FloorToInt(playTime / 60)}m {Mathf.FloorToInt(playTime % 60)}s";
    }

    public void ResetGame()
    {
        keysCollected = 0;
        enemiesKilled = 0;
        playTime = 0f;
        isGameActive = true;
        hasWon = false;              // ← сбрасываем флаг победы
        
        statsPanel.SetActive(false);
    }
}
