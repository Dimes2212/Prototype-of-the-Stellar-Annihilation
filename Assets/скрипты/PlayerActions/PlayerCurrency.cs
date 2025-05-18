using UnityEngine;
using TMPro;

public class PlayerCurrency : MonoBehaviour
{
    public int startCurrency = 0;
    public TextMeshProUGUI currencyText;
    [HideInInspector]
    public int currentCurrency;
    void Awake()
    {
        currentCurrency = startCurrency;
        UpdateUI();
    }
    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateUI();
        Debug.Log($"PlayerCurrency: +{amount}, total = {currentCurrency}");
    }
    public bool SpendMoney(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            UpdateUI();
            Debug.Log($"PlayerCurrency: -{amount}, осталось = {currentCurrency}");
            return true;
        }
        else
        {
            Debug.LogWarning("Не хватает денег для списания!");
            return false;
        }
    }
    public int GetCurrency() => currentCurrency;
    private void UpdateUI()
    {
        if (currencyText != null)
            currencyText.text = $"$ {currentCurrency}";
    }
}
