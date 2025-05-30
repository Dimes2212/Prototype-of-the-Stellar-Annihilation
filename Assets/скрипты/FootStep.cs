using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FootstepSystem : MonoBehaviour
{

    public AudioSource audioSource;
    public float minSpeed = 0.3f;
    public float maxPitch = 1.2f;
    public float startDelay = 0.2f;
    public LayerMask floorLayer;
    public float checkHeight = 1.8f;
    public float maxCheckDistance = 0.5f;

    private XROrigin xrOrigin;
    private Vector3 lastPosition;
    private bool isMoving;
    private float delayTimer;
    private bool hasInitialized;
    private bool isOnValidFloor;

    void Start()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        if (xrOrigin == null)
        {
            Debug.LogError("XROrigin not found in scene!");
            enabled = false;
            return;
        }

        lastPosition = xrOrigin.Camera.transform.position;
        audioSource.Stop();
        delayTimer = startDelay;
        hasInitialized = true;
    }

    void Update()
    {
        if (!hasInitialized) return;

        CheckFloorSurface();

        if (!isOnValidFloor)
        {
            StopFootsteps();
            return;
        }

        UpdateFootsteps();
    }

    private void CheckFloorSurface()
    {
        Vector3 rayStart = xrOrigin.Camera.transform.position - new Vector3(0, checkHeight, 0);
        RaycastHit hit;

        Debug.DrawRay(rayStart, Vector3.down * maxCheckDistance, Color.blue);

        if (Physics.Raycast(rayStart, Vector3.down, out hit, maxCheckDistance, floorLayer))
        {
            isOnValidFloor = true;
        }
        else
        {
            if (Physics.SphereCast(rayStart, 0.2f, Vector3.down, out hit, maxCheckDistance, floorLayer))
            {
                isOnValidFloor = true;
            }
            else
            {
                isOnValidFloor = false;
            }
        }
    }

    private void UpdateFootsteps()
    {
        Vector3 currentPos = xrOrigin.Camera.transform.position;
        Vector3 flatPos = new Vector3(currentPos.x, 0, currentPos.z);
        float speed = Vector3.Distance(flatPos, lastPosition) / Time.deltaTime;

        if (speed > minSpeed)
        {
            if (!isMoving)
            {
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0)
                {
                    PlayFootstep();
                    isMoving = true;
                }
            }
            else
            {
                AdjustPitch(speed);
            }
        }
        else
        {
            StopFootsteps();
        }

        lastPosition = flatPos;
    }

    private void PlayFootstep()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(0.9f, maxPitch);
            audioSource.Play();
        }
    }

    private void AdjustPitch(float speed)
    {
        audioSource.pitch = Mathf.Lerp(1f, maxPitch, speed / 2f);
    }

    private void StopFootsteps()
    {
        if (isMoving)
        {
            audioSource.Stop();
            isMoving = false;
            delayTimer = startDelay;
        }
    }
}