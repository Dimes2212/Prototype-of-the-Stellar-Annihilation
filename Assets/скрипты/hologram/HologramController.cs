using UnityEngine;
using UnityEngine.InputSystem;

public class HologramController : MonoBehaviour
{
    [Header("Hologram Settings")]
    public GameObject hologramUI;
    public InputAction toggleHologramAction;

    private bool isActive = false;

    private void OnEnable()
    {
        toggleHologramAction.Enable();
        toggleHologramAction.performed += Toggle;
    }

    private void OnDisable()
    {
        toggleHologramAction.performed -= Toggle;
        toggleHologramAction.Disable();
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        if (hologramUI != null)
            hologramUI.SetActive(isActive);
    }
}
