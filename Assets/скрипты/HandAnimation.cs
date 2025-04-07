using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    XRInputValueReader<float> m_TriggerInput;
    [SerializeField]
    XRInputValueReader<float> m_GripInput;

    [SerializeField] Animator animator;

    private void Update()
    {
        Debug.Log(m_TriggerInput.ReadValue() + " " +  m_GripInput.ReadValue());
        animator.SetFloat("Trigger", m_TriggerInput.ReadValue());
        animator.SetFloat("Grip", m_GripInput.ReadValue());
    }
}
