using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [Header("Настройки покупки")]
    public int itemCost = 100;
    public GameObject itemPrefab;
    public Transform spawnPoint;

    [Header("Уникальность предмета")]
    public string uniqueTag = "Weapon"; // Предметы с этим тегом будут удалены перед спавном

    private PlayerCurrency playerCurrency;

    void Start()
    {
        // Получаем ссылку на PlayerCurrency
        playerCurrency = FindObjectOfType<PlayerCurrency>();

        // Привязываем кнопку
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PurchaseItem);
        }
        else
        {
            Debug.LogWarning("Компонент Button не найден на объекте.");
        }
    }

    public void PurchaseItem()
    {
        if (playerCurrency == null)
        {
            Debug.LogWarning("PlayerCurrency не найден!");
            return;
        }

        if (playerCurrency.GetCurrency() < itemCost)
        {
            Debug.Log("Недостаточно средств.");
            return;
        }

        // Удаляем все объекты с уникальным тегом, чтобы сохранить единственность
        GameObject[] existingItems = GameObject.FindGameObjectsWithTag(uniqueTag);
        foreach (GameObject item in existingItems)
        {
            Destroy(item);
        }

        // Вычитаем деньги
        playerCurrency.AddCurrency(-itemCost);

        // Спавним новый предмет
        if (itemPrefab != null && spawnPoint != null)
        {
            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Куплен предмет за {itemCost}");
        }
        else
        {
            Debug.LogWarning("ItemPrefab или SpawnPoint не назначены!");
        }
    }
}
