//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;


//    public int keysToWin = 4;


//    public int keysCollected { get; private set; }
//    public int enemiesKilled { get; private set; }
//    public float playTime { get; private set; }


//    public GameObject statsPanel;
//    public TextMeshProUGUI keysText;
//    public TextMeshProUGUI killsText;
//    public TextMeshProUGUI timeText;

//    private bool isGameActive = true;
//    private bool hasWon = false; // ← добавляем флаг победы

//    private void Awake()
//    {
//        Time.timeScale = 1f;
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {

//        }
//    }

//    private void Update()
//    {
//        if (isGameActive)
//        {
//            playTime += Time.deltaTime;
//        }
//    }

//    public void AddKey()
//    {
//        keysCollected++;
//        Debug.Log($"Ключей собрано: {keysCollected}");
//        CheckWinCondition();
//    }


//    public void AddKill()
//    {
//        enemiesKilled++;
//    }

//    private void CheckWinCondition()
//    {
//        if (hasWon) return; // ← предотвращаем повторный вызов

//        if (keysCollected >= keysToWin)
//        {
//            hasWon = true;           // ← ставим флаг победы
//            ShowStats();
//            // Time.timeScale = 0f;  ← УБРАНО! Игра НЕ ставится на паузу
//        }
//    }

//    private void ShowStats()
//    {
//        statsPanel.SetActive(true);
//        keysText.text = $"KEYS: {keysCollected}/{keysToWin}";
//        killsText.text = $"KILLS: {enemiesKilled}";
//        timeText.text = $"TIME: {Mathf.FloorToInt(playTime / 60)}m {Mathf.FloorToInt(playTime % 60)}s";
//    }

//    public void ResetGame()
//    {
//        keysCollected = 0;
//        enemiesKilled = 0;
//        playTime = 0f;
//        isGameActive = true;
//        hasWon = false;              // ← сбрасываем флаг победы

//        statsPanel.SetActive(false);
//    }
//}


using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Progress Settings")]
    public int keysToWin = 4;

    [Header("UI Settings")]
    public GameObject statsPanel;
    public TextMeshProUGUI keysText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timeText;
    public GameObject pauseMenuUI;

    [Header("Player Settings")]
    public GameObject player;

    [Header("Scene Transition Settings")]
    public string loadingSceneName = "LoadingScene";
    public float minLoadingTime = 1.5f;
    public InputActionReference pauseAction;
    public string[] restrictedTags;

    // Game state variables
    public int keysCollected { get; private set; }
    public int enemiesKilled { get; private set; }
    public float playTime { get; private set; }

    private bool isGameActive = true;
    private bool hasWon = false;
    private bool isPaused = false;
    private bool isChangingScene = false;
    private List<Collider> disabledColliders = new();
    private List<Rigidbody> affectedRigidbodies = new();

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
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePauseSystem();
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePressed;
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }
    }

    private void Update()
    {
        if (isGameActive)
        {
            playTime += Time.deltaTime;
        }

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && !isChangingScene)
        {
            TogglePause();
        }
    }

    #region Game Progress Management
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
        if (hasWon) return;

        if (keysCollected >= keysToWin)
        {
            hasWon = true;
            ShowStats();
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
        hasWon = false;

        statsPanel.SetActive(false);
    }
    #endregion

    #region Pause/Resume Functions
    private void InitializePauseSystem()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
        TogglePlayerMovement(true);
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (!isChangingScene)
            TogglePause();
    }

    private void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        isGameActive = false;
        TogglePlayerMovement(false);
        DisableInteractionsByTags(restrictedTags);
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        isGameActive = true;
        TogglePlayerMovement(true);
        EnablePreviouslyDisabledObjects();
    }

    private void TogglePlayerMovement(bool state)
    {
        if (player == null) return;

        var locomotion = player.GetComponent<LocomotionSystem>();
        if (locomotion) locomotion.enabled = state;

        var continuousMove = player.GetComponent<ContinuousMoveProviderBase>();
        if (continuousMove) continuousMove.enabled = state;

        var direct = player.GetComponentsInChildren<XRDirectInteractor>();
        foreach (var di in direct) di.enabled = state;

        var ray = player.GetComponentsInChildren<XRRayInteractor>();
        foreach (var ri in ray) ri.enabled = state;
    }

    private void DisableInteractionsByTags(string[] tags)
    {
        disabledColliders.Clear();
        affectedRigidbodies.Clear();

        foreach (var tag in tags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (var obj in objects)
            {
                Collider[] colliders = obj.GetComponentsInChildren<Collider>();
                foreach (var col in colliders)
                {
                    if (col.enabled)
                    {
                        col.enabled = false;
                        disabledColliders.Add(col);
                    }
                }

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.isKinematic = true;
                    affectedRigidbodies.Add(rb);
                }
            }
        }
    }

    private void EnablePreviouslyDisabledObjects()
    {
        foreach (var col in disabledColliders)
            if (col != null) col.enabled = true;

        foreach (var rb in affectedRigidbodies)
            if (rb != null) rb.isKinematic = false;

        disabledColliders.Clear();
        affectedRigidbodies.Clear();
    }
    #endregion

    #region Scene Transition Functions
    public void LoadScene(string sceneName)
    {
        if (!isChangingScene)
        {
            isChangingScene = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
            isChangingScene = false;
        }
    }

    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        if (!isChangingScene)
        {
            isChangingScene = true;
            StartCoroutine(LoadSceneWithLoadingRoutine(sceneName));
        }
    }

    private IEnumerator LoadSceneWithLoadingRoutine(string sceneName)
    {
        if (isPaused) Resume();

        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncLoad.isDone || timer < minLoadingTime)
        {
            timer += Time.deltaTime;

            if (asyncLoad.progress >= 0.9f && timer >= minLoadingTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        isChangingScene = false;
    }

    public void RestartCurrentScene()
    {
        LoadSceneWithLoadingScreen(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        LoadSceneWithLoadingScreen("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion
}