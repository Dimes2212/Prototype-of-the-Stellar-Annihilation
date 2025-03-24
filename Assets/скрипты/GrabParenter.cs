using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabParent : MonoBehaviour
{
    public void OnGrab(SelectEnterEventArgs args)
    {
        args.interactorObject.transform.SetParent(args.interactorObject.transform);
    }
    public void OnUngrab(SelectExitEventArgs args)
    {
        args.interactorObject.transform.SetParent(null);
    }
}
