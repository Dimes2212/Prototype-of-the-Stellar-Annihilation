using UnityEngine;
using UnityEngine.InputSystem; // Для нового Input System

public class HologramToggle : MonoBehaviour
{
    public GameObject hologramObject;
    public InputActionProperty showButton;

    private bool isShown = false;

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            isShown = !isShown;
            hologramObject.SetActive(isShown);
        }
    }
}
