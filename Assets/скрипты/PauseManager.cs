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

    [Header("Input")]
    public InputActionReference pauseAction; // Можно назначить в инспекторе, но Esc всегда работает

    [Header("Interaction Tag")]
    public string interactableTag = "Interactable"; // Тег, с которым можно взаимодействовать во время паузы

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
        SetPlayerEnabled(true);
        SetInteractionState(true); // Разрешаем взаимодействие с объектами
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
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    private void OnPausePressed(InputAction.CallbackContext ctx) => TogglePause();

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
        SetPlayerEnabled(false);
        SetInteractionState(false); // Ограничиваем взаимодействие только с объектами с нужным тегом
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        SetPlayerEnabled(true);
        SetInteractionState(true); // Разрешаем взаимодействие с объектами
    }

    private void SetPlayerEnabled(bool state)
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

        // Ray Interactors (оставляем всегда активными)
        var ray = player.GetComponentsInChildren<XRRayInteractor>();
        foreach (var ri in ray)
        {
            ri.enabled = true; // Сохраняем лучи активными, даже во время паузы
        }
    }

    // Функция для настройки взаимодействия с объектами на основе тегов
    private void SetInteractionState(bool state)
    {
        if (player == null) return;

        var interactors = player.GetComponentsInChildren<XRBaseInteractor>();
        foreach (var interactor in interactors)
        {
            if (interactor is XRRayInteractor)
            {
                // Оставляем лучи активными
                interactor.enabled = true;
            }
            else
            {
                // Если взаимодействие активно, даем доступ ко всем объектам, иначе только с нужным тегом
                if (state)
                {
                    // Разрешаем взаимодействие со всеми объектами
                    interactor.enabled = true;
                }
                else
                {
                    // Ограничиваем взаимодействие только с объектами, имеющими тег "Interactable"
                    var isInteractable = interactor.gameObject.CompareTag(interactableTag);
                    interactor.enabled = isInteractable;
                }
            }
        }
    }
}
