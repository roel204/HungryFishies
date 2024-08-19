using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject foodPrefab;
    public GameObject betterFoodPrefab;

    private int coinBaseAmount = 2;
    private int coinUpgradeLevel = 0;
    private int coinsPerLevel = 1;
    private int coinRespawnTimer = 3;

    private int foodBaseAmount = 10;
    private int foodUpgradeLevel = 0;
    private int foodPerLevel = 3;
    private int foodRespawnTimer = 3;

    private int betterFoodBaseAmount = 0;
    private int betterFoodUpgradeLevel = 0;
    private int betterFoodPerLevel = 2;
    private int betterFoodRespawnTimer = 3;

    private List<GameObject> spawnedCoins = new List<GameObject>();
    private List<GameObject> spawnedFood = new List<GameObject>();
    private List<GameObject> spawnedBetterFood = new List<GameObject>();

    private ShopManager shopManager;

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        SpawnCoins();
        SpawnFood();
    }

    private void Update()
    {
        int coinUpgradeFromShop = shopManager.shopItems[3, 4];
        if (coinUpgradeFromShop != coinUpgradeLevel)
        {
            coinUpgradeLevel = coinUpgradeFromShop;
            SpawnCoins();
        }

        int foodUpgradeFromShop = shopManager.shopItems[3, 2];
        if (foodUpgradeFromShop != foodUpgradeLevel)
        {
            foodUpgradeLevel = foodUpgradeFromShop;
            SpawnFood();
        }

        int betterFoodUpgradeFromShop = shopManager.shopItems[3, 5];
        if (betterFoodUpgradeFromShop != betterFoodUpgradeLevel)
        {
            betterFoodUpgradeLevel = betterFoodUpgradeFromShop;
            SpawnBetterFood();
        }
    }

    private void SpawnCoins()
    {
        int numCoinsToSpawn = coinBaseAmount + (coinUpgradeLevel * coinsPerLevel);
        int numExistingCoins = spawnedCoins.Count;
        int diff = numCoinsToSpawn - numExistingCoins;

        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject coinInstance = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                spawnedCoins.Add(coinInstance);
            }
        }
    }


    private void SpawnFood()
    {
        int numFoodToSpawn = foodBaseAmount + (foodUpgradeLevel * foodPerLevel);
        int numExistingFood = spawnedFood.Count;
        int diff = numFoodToSpawn - numExistingFood;

        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject foodInstance = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
                spawnedFood.Add(foodInstance);
            }
        }
    }

    private void SpawnBetterFood()
    {
        int numBetterFoodToSpawn = betterFoodBaseAmount + (betterFoodUpgradeLevel * betterFoodPerLevel);
        int numExistingBetterFood = spawnedBetterFood.Count;
        int diff = numBetterFoodToSpawn - numExistingBetterFood;

        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject betterFoodInstance = Instantiate(betterFoodPrefab, spawnPosition, Quaternion.identity);
                spawnedBetterFood.Add(betterFoodInstance);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float minX = -8.5f;
        float maxX = 8.5f;
        float minY = -4.5f;
        float maxY = 4f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector3(randomX, randomY, 0f);
    }


    public void StartCoinRespawnTimer(GameObject coin)
    {
        StartCoroutine(CoinRespawnTimer(coin));
    }

    private IEnumerator CoinRespawnTimer(GameObject coin)
    {
        yield return new WaitForSeconds(coinRespawnTimer);

        Vector3 spawnPosition = GetRandomSpawnPosition();
        coin.transform.position = spawnPosition;
        coin.SetActive(true);
    }

    public void StartFoodRespawnTimer(GameObject food)
    {
        StartCoroutine(FoodRespawnTimer(food));
    }

    private IEnumerator FoodRespawnTimer(GameObject food)
    {
        yield return new WaitForSeconds(foodRespawnTimer);

        Vector3 spawnPosition = GetRandomSpawnPosition();
        food.transform.position = spawnPosition;
        food.SetActive(true);
    }

    public void StartBetterFoodRespawnTimer(GameObject betterFood)
    {
        StartCoroutine(BetterFoodRespawnTimer(betterFood));
    }

    private IEnumerator BetterFoodRespawnTimer(GameObject betterFood)
    {
        yield return new WaitForSeconds(betterFoodRespawnTimer);

        Vector3 spawnPosition = GetRandomSpawnPosition();
        betterFood.transform.position = spawnPosition;
        betterFood.SetActive(true);
    }
}
