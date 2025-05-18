using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        CheckWinCondition();
    }

    public void AddKill()
    {
        enemiesKilled++;
    }

    private void CheckWinCondition()
    {
        if (keysCollected >= keysToWin)
        {
            ShowStats();
            isGameActive = false;
            Time.timeScale = 0f;
        }
    }

    private void ShowStats()
    {
        statsPanel.SetActive(true);
        keysText.text = $"Keys: {keysCollected}/{keysToWin}";
        killsText.text = $"Kills: {enemiesKilled}";
        timeText.text = $"Time: {Mathf.FloorToInt(playTime / 60)}m {Mathf.FloorToInt(playTime % 60)}s";
    }

    public void ResetGame()
    {
        keysCollected = 0;
        enemiesKilled = 0;
        playTime = 0f;
        isGameActive = true;
        Time.timeScale = 1f;
        statsPanel.SetActive(false);
    }
}