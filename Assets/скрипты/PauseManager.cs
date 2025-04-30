using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject player;
    public InputActionReference pauseAction; // <-- перетащим сюда в инспекторе

    private bool isPaused = false;

    void OnEnable()
    {
        pauseAction.action.performed += OnPausePressed;
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
        pauseAction.action.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        TogglePlayerMovement(false);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        TogglePlayerMovement(true);
    }

    private void TogglePlayerMovement(bool state)
    {
        var locomotion = player.GetComponent<UnityEngine.XR.Interaction.Toolkit.LocomotionSystem>();
        if (locomotion != null)
            locomotion.enabled = state;

        var continuousMove = player.GetComponent<UnityEngine.XR.Interaction.Toolkit.ContinuousMoveProviderBase>();
        if (continuousMove != null)
            continuousMove.enabled = state;

        var directInteractors = player.GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();
        foreach (var interactor in directInteractors)
            interactor.enabled = state;

        var rayInteractors = player.GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        foreach (var interactor in rayInteractors)
            interactor.enabled = state;
    }
}
