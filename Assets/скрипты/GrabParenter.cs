using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabParent : MonoBehaviour
{
    public void OnGrab(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.SetParent(args.interactableObject.transform);
    }
    public void OnUngrab(SelectExitEventArgs args)
    {
        args.interactableObject.transform.SetParent(null);
    }
}
