using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class FishData
{
    public int index;
    public string name;
    public string abilityText;
    public string[] abilities;
    public int cost;
    public int defaultSpeed;
    public int defaultRotate;
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
    public int money;
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

        money = PlayerPrefs.GetInt("Money");

        // Load the JSON file from Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("fishData"); // Do not include file extension
        string jsonData = jsonFile.text;

        // Parse the JSON data into a list of FishData objects
        fishDataList = JsonUtility.FromJson<FishDataWrapper>(jsonData).fishData;
    }

    public void GameOver()
    {
        timer = FindObjectOfType<Timer>();
        score = timer.GetTimerValue();
    }

    public bool ChangeMoney(int amount)
    {
        if (money + amount >= 0)
        {
            money += amount;
            PlayerPrefs.SetInt("Money", money);
            return true;
        }
        else
        {
            return false;
        }
    }
}
