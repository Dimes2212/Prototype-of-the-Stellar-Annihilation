using UnityEngine;
using UnityEngine.UI;

public class TurretShopButton : MonoBehaviour
{
    
    public int turretCost = 150;
    public GameObject turretPrefab;

    
    public Transform[] turretSpawnPoints;

    private int currentSpawnIndex = 0;
    private PlayerCurrency playerCurrency;

    void Start()
    {
        playerCurrency = FindObjectOfType<PlayerCurrency>();

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PurchaseTurret);
        }
        else
        {
            Debug.LogWarning("Button не найден!");
        }
    }

    public void PurchaseTurret()
    {
        if (playerCurrency == null)
        {
            Debug.LogWarning("PlayerCurrency не найден!");
            return;
        }

        if (currentSpawnIndex >= turretSpawnPoints.Length)
        {
            Debug.Log("Все турельные точки заняты.");
            return;
        }

        if (playerCurrency.GetCurrency() < turretCost)
        {
            Debug.Log("Недостаточно средств для покупки турели.");
            return;
        }

        Transform spawnPoint = turretSpawnPoints[currentSpawnIndex];

        if (spawnPoint != null && turretPrefab != null)
        {
            Instantiate(turretPrefab, spawnPoint.position, spawnPoint.rotation);
            playerCurrency.AddCurrency(-turretCost);
            Debug.Log($"Турель установлена в точке {currentSpawnIndex + 1}, осталось точек: {turretSpawnPoints.Length - currentSpawnIndex - 1}");

            currentSpawnIndex++;
        }
        else
        {
            Debug.LogWarning("SpawnPoint или TurretPrefab не назначены.");
        }
    }
}
