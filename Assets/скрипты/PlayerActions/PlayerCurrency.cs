using UnityEngine;
using TMPro;

public class PlayerCurrency : MonoBehaviour
{
    [Header("Начальный баланс")]
    public int startCurrency = 0;

    [Header("UI (голограмма)")]
    public TextMeshProUGUI currencyText; // Привяжи сюда твой голографический UI

    private int currentCurrency;

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

    public int GetCurrency() => currentCurrency;

    private void UpdateUI()
    {
        if (currencyText != null)
            currencyText.text = $"$ {currentCurrency}";
    }
}
