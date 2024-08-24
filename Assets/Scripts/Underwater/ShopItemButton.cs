using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceTxt;
    public TextMeshProUGUI NameTxt;
    public Slider slider;
    public ShopManager ShopManager;

    private int maxUpgrade;
    private int currentUpgrade = 0;

    private void Start()
    {
        ShopManager = FindObjectOfType<ShopManager>();

        NameTxt.text = ShopManager.shopItemNames[1, ItemID];
        maxUpgrade = ShopManager.maxUpgradeLevel;
        slider.maxValue = maxUpgrade;
        slider.value = currentUpgrade;
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        int level = ShopManager.shopItems[3, ItemID];
        int price = ShopManager.CalculateCost(ItemID, level);

        if (level < maxUpgrade)
        {
            slider.value = level;
            PriceTxt.text = "Price: " + price.ToString();
        }
        else
        {
            slider.value = maxUpgrade;
            PriceTxt.text = "Price: Max";
        }
    }
}
