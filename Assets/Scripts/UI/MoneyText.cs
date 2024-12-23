using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
        GameManager.instance.OnMoneyChanged += UpdateMoneyUI;
        UpdateMoneyUI();
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnMoneyChanged -= UpdateMoneyUI;
        }
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = GameManager.instance.money.ToString();
    }

}
