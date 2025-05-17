using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Settings")]
    public GameObject pauseMenuUI;
    public GameObject player;
    public InputActionReference pauseAction;
    public string[] restrictedTags;

    [Header("Scene Transition Settings")]
    public string loadingSceneName = "LoadingScene";
    public float minLoadingTime = 1.5f;

    private bool isPaused = false;
    private bool isChangingScene = false;
    private List<Collider> disabledColliders = new();
    private List<Rigidbody> affectedRigidbodies = new();

    void Start()
    {
        InitializePauseSystem();
    }

    void InitializePauseSystem()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
        TogglePlayerMovement(true);
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePressed;
            pauseAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && !isChangingScene)
            TogglePause();
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (!isChangingScene)
            TogglePause();
    }

    #region Pause/Resume Functions
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
        TogglePlayerMovement(false);
        DisableInteractionsByTags(restrictedTags);
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
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
            Time.timeScale = 1f; // Сбрасываем таймскейл перед загрузкой
            SceneManager.LoadScene(sceneName);
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
        // Сбрасываем паузу перед загрузкой
        if (isPaused) Resume();

        // Загружаем сцену загрузки
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        // Ждём минимум один кадр
        yield return null;

        // Начинаем загрузку целевой сцены
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
    #endregion

    #region UI Button Handlers
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