using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HologramController : MonoBehaviour
{
    public CanvasGroup hologramCanvasGroup;
    public InputActionProperty showHologramAction; // Сюда закинем экшен кнопки
    public float fadeSpeed = 5f;

    private bool isVisible = false;

    private void Update()
    {
        if (showHologramAction.action.WasPressedThisFrame())
        {
            ToggleHologram();
        }

        // Плавное появление/исчезновение
        float targetAlpha = isVisible ? 1f : 0f;
        hologramCanvasGroup.alpha = Mathf.Lerp(hologramCanvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        // Блокируем клики если голограмма невидима
        hologramCanvasGroup.blocksRaycasts = isVisible;
        hologramCanvasGroup.interactable = isVisible;
    }

    private void ToggleHologram()
    {
        isVisible = !isVisible;
    }
}
