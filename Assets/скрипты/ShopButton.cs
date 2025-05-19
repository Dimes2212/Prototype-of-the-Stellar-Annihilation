//using UnityEngine;
//using UnityEngine.UI;

//public class ShopButton : MonoBehaviour
//{
//    [Header("Настройки покупки")]
//    public int itemCost = 100;
//    public GameObject itemPrefab;
//    public Transform spawnPoint;

//    [Header("Уникальность предмета")]
//    public string uniqueTag = "Weapon"; // Предметы с этим тегом будут удалены перед спавном

//    private PlayerCurrency playerCurrency;

//    void Start()
//    {
//        // Получаем ссылку на PlayerCurrency
//        playerCurrency = FindObjectOfType<PlayerCurrency>();

//        // Привязываем кнопку
//        Button button = GetComponent<Button>();
//        if (button != null)
//        {
//            button.onClick.AddListener(PurchaseItem);
//        }
//        else
//        {
//            Debug.LogWarning("Компонент Button не найден на объекте.");
//        }
//    }

//    public void PurchaseItem()
//    {
//        if (playerCurrency == null)
//        {
//            Debug.LogWarning("PlayerCurrency не найден!");
//            return;
//        }

//        if (playerCurrency.GetCurrency() < itemCost)
//        {
//            Debug.Log("Недостаточно средств.");
//            return;
//        }

//        // Удаляем все объекты с уникальным тегом, чтобы сохранить единственность
//        GameObject[] existingItems = GameObject.FindGameObjectsWithTag(uniqueTag);
//        foreach (GameObject item in existingItems)
//        {
//            Destroy(item);
//        }

//        // Вычитаем деньги
//        playerCurrency.AddCurrency(-itemCost);

//        // Спавним новый предмет
//        if (itemPrefab != null && spawnPoint != null)
//        {
//            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
//            Debug.Log($"Куплен предмет за {itemCost}");
//        }
//        else
//        {
//            Debug.LogWarning("ItemPrefab или SpawnPoint не назначены!");
//        }
//    }
//}


using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [Header("Настройки покупки")]
    public int itemCost = 100;
    public GameObject itemPrefab;
    public Transform spawnPoint;

    [Header("Уникальность предмета")]
    public string uniqueTag = "Weapon";

    private PlayerCurrency playerCurrency;
    private Button button;

    void Start()
    {
        // Инициализация кнопки
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

        // Проверка ссылок
        if (itemPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Не назначены itemPrefab или spawnPoint!", this);
            button.interactable = false;
            return;
        }

        // Подписка на событие
        button.onClick.AddListener(PurchaseItem);
    }

    public void PurchaseItem()
    {
        // Проверка средств
        if (playerCurrency.GetCurrency() < itemCost)
        {
            Debug.Log("Недостаточно средств.");
            return;
        }

        // Удаление старых предметов
        if (!string.IsNullOrEmpty(uniqueTag))
        {
            var existingItems = GameObject.FindGameObjectsWithTag(uniqueTag);
            foreach (var item in existingItems)
            {
                if (item != null) Destroy(item);
            }
        }

        // Спавн нового предмета
        try
        {
            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
            playerCurrency.AddCurrency(-itemCost);
            Debug.Log($"Успешная покупка: {itemPrefab.name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка при покупке: {e.Message}");
        }
    }

    void OnDestroy()
    {
        // Отписка от событий
        if (button != null)
            button.onClick.RemoveListener(PurchaseItem);
    }
}