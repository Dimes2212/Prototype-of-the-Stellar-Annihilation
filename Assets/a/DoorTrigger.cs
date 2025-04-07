using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
    public Transform door; // Ссылка на объект двери
    public Vector3 moveOffset = new Vector3(0, 0, -1f); // Насколько сместить дверь внутрь стены
    public float moveSpeed = 2f; // Скорость движения двери
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
            targetPosition = initialPosition + moveOffset;
        }
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

        door.position = target; // Финальное положение
    }
}
