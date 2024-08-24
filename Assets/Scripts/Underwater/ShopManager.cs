using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI coinCounter;

    public int coins = 0;
    public int[,] shopItems = new int[5, 8];
    public string[,] shopItemNames = new string[2, 8];
    public int maxUpgradeLevel = 5;

    private SFXManager sfxManager;

    private void Awake()
    {
        // Name
        shopItemNames[1, 1] = "Swim Faster";
        shopItemNames[1, 2] = "More Food";
        shopItemNames[1, 3] = "Bigger Fish";
        shopItemNames[1, 4] = "More Coins";
        shopItemNames[1, 5] = "Better Food";
        shopItemNames[1, 6] = "More HP";
        shopItemNames[1, 7] = "Rotate Speed";

        if (GameManager.instance.selectedFish == 7 || GameManager.instance.selectedFish == 8 || GameManager.instance.selectedFish == 11)
        {
            maxUpgradeLevel = 8;
        }
    }

    void Start()
    {
        sfxManager = FindObjectOfType<SFXManager>();

        // ID
        shopItems[1, 1] = 1; // Swim Speed
        shopItems[1, 2] = 2; // More Food
        shopItems[1, 3] = 3; // Bigger Fish
        shopItems[1, 4] = 4; // More Coins
        shopItems[1, 5] = 5; // Better Food
        shopItems[1, 6] = 6; // More HP
        shopItems[1, 7] = 7; // Rotate Speed

        // Price
        shopItems[2, 1] = 2;
        shopItems[2, 2] = 3;
        shopItems[2, 3] = 1;
        shopItems[2, 4] = 5;
        shopItems[2, 5] = 4;
        shopItems[2, 6] = 7;
        shopItems[2, 7] = 2;

        // Level
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
        shopItems[3, 6] = 0;
        shopItems[3, 7] = 0;

        // Price Addition
        shopItems[4, 1] = 2;
        shopItems[4, 2] = 2;
        shopItems[4, 3] = 2;
        shopItems[4, 4] = 3;
        shopItems[4, 5] = 3;
        shopItems[4, 6] = 4;
        shopItems[4, 7] = 2;
    }

    public void IncreaseCoinCount(float amount)
    {
        coins += (int)amount;
        coinCounter.text = "Coins: " + coins;
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        int itemID = ButtonRef.GetComponent<ShopItemButton>().ItemID;
        int currentLevel = shopItems[3, itemID];

        if (currentLevel < maxUpgradeLevel)
        {
            int cost = CalculateCost(itemID, currentLevel);
            if (coins >= cost)
            {
                IncreaseCoinCount(-cost);
                shopItems[3, itemID]++;
                sfxManager.PlaySFXBuy();
            }
            else { sfxManager.PlaySFXError(); }
        }
    }

    public int CalculateCost(int itemID, int currentLevel)
    {
        int baseCost = shopItems[2, itemID];
        int incrementalCost = shopItems[4, itemID];
        int nextLevelCost = baseCost + (incrementalCost * currentLevel);

        // Half cost if sardine
        if (GameManager.instance.selectedFish == 6)
        {
            int halfCost = nextLevelCost / 2;
            return halfCost;
        }

        return nextLevelCost;
    }
}
