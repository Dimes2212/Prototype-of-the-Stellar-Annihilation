using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI;          // Твоё паузное меню (Canvas)

    [Header("Player")]
    public GameObject player;               // Твой XR Origin или родитель предметов управления

    [Header("Input (необязательно)")]
    public InputActionReference pauseAction; // Можно назначить в инспекторе, но Esc всегда работает

    private bool isPaused = false;

    void Start()
    {
        // Скрываем меню при старте
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Снимаем паузу, включаем время и передвижение
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
        // Всегда ловим Escape на клавиатуре
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        Debug.Log("== PAUSE ==");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        TogglePlayerMovement(false);
    }

    public void Resume()
    {
        Debug.Log("== RESUME ==");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        TogglePlayerMovement(true);
    }

    private void TogglePlayerMovement(bool state)
    {
        if (player == null) return;

        // XR Locomotion
        var locomotion = player.GetComponent<LocomotionSystem>();
        if (locomotion) locomotion.enabled = state;

        var continuousMove = player.GetComponent<ContinuousMoveProviderBase>();
        if (continuousMove) continuousMove.enabled = state;

        // Direct Interactors
        var direct = player.GetComponentsInChildren<XRDirectInteractor>();
        foreach (var di in direct) di.enabled = state;

        // Ray Interactors
        var ray = player.GetComponentsInChildren<XRRayInteractor>();
        foreach (var ri in ray) ri.enabled = state;
    }
}
