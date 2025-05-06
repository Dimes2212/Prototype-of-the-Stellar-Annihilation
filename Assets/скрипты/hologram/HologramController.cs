using UnityEngine;
using UnityEngine.InputSystem;

public class HologramController : MonoBehaviour
{
    
    public GameObject hologramUI;
    public InputAction toggleHologramAction;

    private bool isActive = false;

    private void OnEnable()
    {
        toggleHologramAction.Enable();
        toggleHologramAction.canceled += Toggle; 
    }

    private void OnDisable()
    {
        toggleHologramAction.canceled -= Toggle;
        toggleHologramAction.Disable();
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        if (hologramUI != null)
            hologramUI.SetActive(isActive);
    }
}
