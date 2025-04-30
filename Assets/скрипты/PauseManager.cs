using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Панель меню паузы
    public GameObject player; // XR Rig или корневой объект игрока
    public string mainMenuSceneName = "MainMenu"; // Имя сцены главного меню

    private bool isPaused = false;
    private XRInteractorLineVisual[] interactorLineVisuals; // Для отключения взаимодействия с линиями

    void Start()
    {
        // Скрываем меню паузы при старте игры
        pauseMenuUI.SetActive(false);

        // Получаем все XR Interactors, которые могут управлять перемещением
        interactorLineVisuals = player.GetComponentsInChildren<XRInteractorLineVisual>();
    }

    void Update()
    {
        // Нажатие кнопки для активации меню паузы
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current?.startButton.wasPressedThisFrame == true)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // Открытие меню паузы
    public void Pause()
    {
        // Включаем меню паузы
        pauseMenuUI.SetActive(true);

        // Ставим игру на паузу
        Time.timeScale = 0f;
        isPaused = true;

        // Отключаем перемещение игрока
        TogglePlayerMovement(false);
    }

    // Закрытие меню паузы
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Включаем перемещение игрока
        TogglePlayerMovement(true);
    }

    // Переход в главное меню
    public void LoadMainMenu()
    {
        // Включаем снова нормальную скорость времени (если игрок вернется на сцену)
        Time.timeScale = 1f;

        // Загружаем сцену главного меню
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Отключение или включение перемещения игрока
    private void TogglePlayerMovement(bool state)
    {
        // Пример — отключить компоненты, которые управляют движением игрока
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

        // Отключение взаимодействия с линиями (если есть)
        foreach (var lineVisual in interactorLineVisuals)
        {
            lineVisual.enabled = state;
        }
    }
}
