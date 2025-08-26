using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UpgradeManager : MonoBehaviour {
    public List<Upgrade> upgrades;
    public GameObject shopItemPrefab;
    public Transform shopContainer;
    public GameObject upgradeShopCanvas;

    public int coins = 0;
    public TextMeshProUGUI coinCounter;

    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();
    private Dictionary<string, UpgradeItemUI> upgradeItemUIs = new Dictionary<string, UpgradeItemUI>();

    private void Start() {
        InitializeShop();
    }

    private void InitializeShop() {
        foreach (var upgrade in upgrades) {
            // Initialize upgrade level to 0
            upgradeLevels[upgrade.id] = 0;

            // Instantiate the prefab
            GameObject shopItem = Instantiate(shopItemPrefab, shopContainer);

            // Update the prefab UI with upgrade data
            var shopItemUI = shopItem.GetComponent<UpgradeItemUI>();
            shopItemUI.Initialize(upgrade, () => Upgrade(upgrade.id));

            // Store reference to the UpgradeItemUI for updates
            upgradeItemUIs[upgrade.id] = shopItemUI;
        }
    }

    public void IncreaseCoinCount(float amount) {
        coins += (int)amount;
        coinCounter.text = "Coins: " + coins;
    }

    public void Upgrade(string upgradeId) {
        if (!upgradeLevels.ContainsKey(upgradeId)) return;

        var upgrade = upgrades.Find(u => u.id == upgradeId);
        int currentLevel = upgradeLevels[upgradeId];

        if (currentLevel < upgrade.maxLevel) {
            int cost = CalculateCost(upgradeId);
            if (coins >= cost) {
                IncreaseCoinCount(-cost);
                upgradeLevels[upgradeId]++;

                // Update the corresponding UI
                if (upgradeItemUIs.TryGetValue(upgradeId, out var itemUI)) {
                    itemUI.UpdateUI(upgradeLevels[upgradeId]);
                }

                Debug.Log($"{upgrade.upgradeName} upgraded to level {upgradeLevels[upgradeId]}");
                SoundManager.Instance.PlaySound("Sfx", "buy");
            } else {
                SoundManager.Instance.PlaySound("Sfx", "error");
            }
        }
    }

    public int CalculateCost(string upgradeId) {
        var upgrade = upgrades.Find(u => u.id == upgradeId);
        int currentLevel = upgradeLevels[upgradeId];

        if (GameManager.instance.fishDataList[GameManager.instance.selectedFish].abilities.Contains("cheapUpgrades")) {
            return (upgrade.baseCost + (currentLevel * upgrade.priceIncrement)) / 2;
        }

        return upgrade.baseCost + (currentLevel * upgrade.priceIncrement);
    }

    public int GetLevel(string upgradeId) {
        return upgradeLevels.ContainsKey(upgradeId) ? upgradeLevels[upgradeId] : 0;
    }
}
