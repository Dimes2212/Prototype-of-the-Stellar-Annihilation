using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class DoorTriggerLocal : MonoBehaviour
{
    public Transform door;
    public Vector3 localMoveOffset = new Vector3(0, 0, -1f);
    public float moveSpeed = 2f;
    public AudioSource doorAudio;
    public AudioClip openSound;
    public AudioClip closeSound;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Coroutine doorCoroutine;
    private bool isOpen = false;

    void Start()
    {
        if (door != null)
        {
            initialPosition = door.position;
            targetPosition = initialPosition + door.TransformDirection(localMoveOffset);

            // ✅ Убедись, что дверь физически перемещается — добавим Rigidbody
            Rigidbody rb = door.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = door.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        // Убедимся, что триггер включен
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(targetPosition));

            if (doorAudio != null && openSound != null)
            {
                doorAudio.clip = openSound;
                doorAudio.Play();
            }

            isOpen = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(initialPosition));

            if (doorAudio != null && closeSound != null)
            {
                doorAudio.clip = closeSound;
                doorAudio.Play();
            }

            isOpen = false;
        }
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        Vector3 start = door.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            door.position = Vector3.Lerp(start, target, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        door.position = target;
    }
}
