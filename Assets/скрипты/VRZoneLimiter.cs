using UnityEngine;
using TMPro;

public class VRZoneLimiter : MonoBehaviour
{
    [Header("Точка возврата")]
    public Transform basePoint;               // Точка, куда телепортировать

    [Header("Игроки")]
    public string playerTag = "Player";       // Тег объектов, которые считаем «игроками»

    [Header("Предупреждение")]
    public GameObject warningCanvas;          // Канвас с предупреждением
    public TextMeshProUGUI warningText;       // Текст с отсчётом времени

    [Header("Настройки")]
    public float delayBeforeTeleport = 5f;    // Задержка до телепортации

    private bool isPlayerOutside = false;
    private float exitTimer = 0f;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && !isPlayerOutside)
        {
            isPlayerOutside = true;
            exitTimer = 0f;
            if (warningCanvas != null)
                warningCanvas.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && isPlayerOutside)
        {
            isPlayerOutside = false;
            exitTimer = 0f;
            if (warningCanvas != null)
                warningCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (!isPlayerOutside) return;

        exitTimer += Time.deltaTime;
        float remainingTime = Mathf.Ceil(delayBeforeTeleport - exitTimer);

        if (warningText != null)
            warningText.text = $"<color=red>Вы покинули игровую зону!</color>\nВозврат через: {remainingTime} сек.";

        if (exitTimer >= delayBeforeTeleport)
        {
            TeleportAllPlayers();
            ResetWarning();
        }
    }

    private void TeleportAllPlayers()
    {
        if (basePoint == null) return;

        var players = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (var player in players)
        {
            player.transform.position = basePoint.position;
            player.transform.rotation = basePoint.rotation;
        }
    }

    private void ResetWarning()
    {
        isPlayerOutside = false;
        exitTimer = 0f;
        if (warningCanvas != null)
            warningCanvas.SetActive(false);
    }
}
