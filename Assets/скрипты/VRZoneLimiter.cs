using UnityEngine;
using TMPro; // если используешь TextMeshPro

public class VRZoneLimiter : MonoBehaviour
{
    public Transform basePoint; // Точка возврата
    public GameObject warningCanvas; // Канвас с предупреждением
    public TextMeshProUGUI warningText; // Текст с отсчетом

    private bool isPlayerOutside = false;
    private float exitTimer = 0f;
    public float delayBeforeTeleport = 5f;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOutside = true;
            exitTimer = 0f;
            if (warningCanvas != null)
                warningCanvas.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOutside = false;
            exitTimer = 0f;
            if (warningCanvas != null)
                warningCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerOutside)
        {
            exitTimer += Time.deltaTime;
            float remainingTime = Mathf.Ceil(delayBeforeTeleport - exitTimer);

            if (warningText != null)
                warningText.text = $"<color=red>Вы покинули игровую зону!</color>\nВозврат через: {remainingTime} сек.";

            if (exitTimer >= delayBeforeTeleport)
            {
                TeleportPlayer();
                isPlayerOutside = false;
                exitTimer = 0f;

                if (warningCanvas != null)
                    warningCanvas.SetActive(false);
            }
        }
    }

    void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && basePoint != null)
        {
            player.transform.position = basePoint.position;
            player.transform.rotation = basePoint.rotation;
        }
    }
}
