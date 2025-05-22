using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FootstepSystem : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    [Tooltip("Minimum movement speed to trigger footsteps")]
    public float minSpeed = 0.3f;
    [Tooltip("Maximum pitch variation")]
    public float maxPitch = 1.2f;
    [Tooltip("Delay before first footstep after movement starts")]
    public float startDelay = 0.2f;

    private XROrigin xrOrigin;
    private Vector3 lastPosition;
    private bool isMoving;
    private float delayTimer;
    private bool hasInitialized;

    void Start()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        lastPosition = xrOrigin.Camera.transform.position;
        audioSource.Stop(); // Гарантируем, что звук не играет при старте
        delayTimer = startDelay;
        hasInitialized = true;
    }

    void Update()
    {
        if (!hasInitialized) return;

        Vector3 currentPos = xrOrigin.Camera.transform.position;
        Vector3 flatPos = new Vector3(currentPos.x, 0, currentPos.z);
        float speed = Vector3.Distance(flatPos, lastPosition) / Time.deltaTime;

        if (speed > minSpeed)
        {
            if (!isMoving)
            {
                // Задержка перед первым шагом
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0)
                {
                    audioSource.Play();
                    isMoving = true;
                }
            }
            else
            {
                audioSource.pitch = Mathf.Lerp(1f, maxPitch, speed / 2f);
            }
        }
        else
        {
            if (isMoving)
            {
                audioSource.Stop();
                isMoving = false;
            }
            delayTimer = startDelay; // Сброс таймера при остановке
        }

        lastPosition = flatPos;
    }
}