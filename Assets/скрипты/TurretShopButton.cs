//using UnityEngine;
//using UnityEngine.UI;

//public class TurretShopButton : MonoBehaviour
//{

//    public int turretCost = 150;
//    public GameObject turretPrefab;


//    public Transform[] turretSpawnPoints;

//    private int currentSpawnIndex = 0;
//    private PlayerCurrency playerCurrency;

//    void Start()
//    {
//        playerCurrency = FindObjectOfType<PlayerCurrency>();

//        Button button = GetComponent<Button>();
//        if (button != null)
//        {
//            button.onClick.AddListener(PurchaseTurret);
//        }
//        else
//        {
//            Debug.LogWarning("Button не найден!");
//        }
//    }

//    public void PurchaseTurret()
//    {
//        if (playerCurrency == null)
//        {
//            Debug.LogWarning("PlayerCurrency не найден!");
//            return;
//        }

//        if (currentSpawnIndex >= turretSpawnPoints.Length)
//        {
//            Debug.Log("Все турельные точки заняты.");
//            return;
//        }

//        if (playerCurrency.GetCurrency() < turretCost)
//        {
//            Debug.Log("Недостаточно средств для покупки турели.");
//            return;
//        }

//        Transform spawnPoint = turretSpawnPoints[currentSpawnIndex];

//        if (spawnPoint != null && turretPrefab != null)
//        {
//            Instantiate(turretPrefab, spawnPoint.position, spawnPoint.rotation);
//            playerCurrency.AddCurrency(-turretCost);
//            Debug.Log($"Турель установлена в точке {currentSpawnIndex + 1}, осталось точек: {turretSpawnPoints.Length - currentSpawnIndex - 1}");

//            currentSpawnIndex++;
//        }
//        else
//        {
//            Debug.LogWarning("SpawnPoint или TurretPrefab не назначены.");
//        }
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class TurretShopButton : MonoBehaviour
{
    [Header("Настройки покупки турели")]
    public int turretCost = 150;
    public GameObject turretPrefab;

    [Header("Точки для установки турелей")]
    public Transform[] turretSpawnPoints;

    private int currentSpawnIndex = 0;
    private PlayerCurrency playerCurrency;
    private Button button;

    void Start()
    {
        // Получаем кнопку
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Компонент Button не найден!", this);
            enabled = false;
            return;
        }

        // Поиск PlayerCurrency
        playerCurrency = FindObjectOfType<PlayerCurrency>();
        if (playerCurrency == null)
        {
            Debug.LogError("PlayerCurrency не найден в сцене!", this);
            button.interactable = false;
            return;
        }

        // Проверка префаба и точек
        if (turretPrefab == null || turretSpawnPoints == null || turretSpawnPoints.Length == 0)
        {
            Debug.LogError("TurretPrefab не назначен или список точек пуст!", this);
            button.interactable = false;
            return;
        }

        // Подписка на кнопку
        button.onClick.AddListener(PurchaseTurret);
    }

    public void PurchaseTurret()
    {
        if (playerCurrency.GetCurrency() < turretCost)
        {
            Debug.Log("Недостаточно средств для покупки турели.");
            return;
        }

        if (currentSpawnIndex >= turretSpawnPoints.Length)
        {
            Debug.Log("Все турельные точки заняты.");
            return;
        }

        Transform spawnPoint = turretSpawnPoints[currentSpawnIndex];

        if (spawnPoint != null)
        {
            try
            {
                Instantiate(turretPrefab, spawnPoint.position, spawnPoint.rotation);
                playerCurrency.AddCurrency(-turretCost);
                Debug.Log($"Турель установлена в точке {currentSpawnIndex + 1} из {turretSpawnPoints.Length}");
                currentSpawnIndex++;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка при установке турели: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"SpawnPoint под индексом {currentSpawnIndex} не назначен.");
        }
    }

    void OnDestroy()
    {
        // Отписка от события
        if (button != null)
            button.onClick.RemoveListener(PurchaseTurret);
    }
}
