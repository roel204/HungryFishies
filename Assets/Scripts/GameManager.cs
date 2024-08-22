using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class FishData
{
    public int index;
    public string name;
    public string ability;
    public int cost;
    public int defaultSpeed;
    public int defaultTurnSpeed;
    public int defaultHealth;
}

[System.Serializable]
public class FishDataWrapper
{
    public List<FishData> fishData;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<FishData> fishDataList; // Make this public so it's accessible from other scripts

    public float score;
    public int pearls;
    public int selectedFish;
    private Timer timer;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pearls = PlayerPrefs.GetInt("Pearls");

        // Load the JSON file
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "fishData.json");
        string jsonData = File.ReadAllText(jsonFilePath);

        // Parse the JSON data into a list of FishData objects
        fishDataList = JsonUtility.FromJson<FishDataWrapper>(jsonData).fishData;
    }

    public void GameOver()
    {
        timer = FindObjectOfType<Timer>();
        score = timer.GetTimerValue();
    }

    public bool ChangePearls(int amount)
    {
        if (pearls + amount >= 0)
        {
            pearls += amount;
            PlayerPrefs.SetInt("Pearls", pearls);

            //sfxManager.PlaySFXBuy();

            return true;
        }
        else
        {
            //sfxManager.PlaySFXError();

            return false;
        }
    }
}
