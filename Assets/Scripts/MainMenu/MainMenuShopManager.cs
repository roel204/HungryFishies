using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;  // Ensure this is included for Path and File

[System.Serializable]
public class FishData
{
    public int index;
    public string name;
    public int cost;
    public int defaultSpeed;
    public int defaultTurnSpeed;
}

[System.Serializable]
public class FishDataWrapper
{
    public List<FishData> fishData;
}

public class MainMenuShopManager : MonoBehaviour
{
    private List<FishData> fishDataList; // List to store fish data loaded from JSON

    public Button purchaseButton;
    public Button playButton;
    public TextMeshProUGUI pearlCounter;
    public Image selectedFishImage; // Reference to the UI Image to display the selected fish
    [SerializeField] private SimpleScrollSnap scrollSnap;
    private SceneHandler sceneHandler;

    private int selectedFishIndex = 0;
    private bool[] purchasedFish; // An array to store purchased fish data

    private void Awake()
    {
        selectedFishIndex = PlayerPrefs.GetInt("LastFish");
        scrollSnap.StartingPanel = selectedFishIndex;
    }

    private void Start()
    {
        sceneHandler = FindFirstObjectByType<SceneHandler>();

        // Load the JSON file
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "fishData.json");
        string jsonData = File.ReadAllText(jsonFilePath);

        // Parse the JSON data into a list of FishData objects
        fishDataList = JsonUtility.FromJson<FishDataWrapper>(jsonData).fishData;

        // Initialize the purchased fish array based on the size of fishDataList
        purchasedFish = new bool[fishDataList.Count];

        LoadPurchasedFishData();
        UpdateShopUI();
    }

    public void SelectFish(int selectedItem, int previousItem)
    {
        selectedFishIndex = selectedItem;
        GameManager.instance.selectedFish = selectedFishIndex;
        Debug.Log(selectedItem + "|" + previousItem);

        UpdateShopUI();
    }

    private void LoadPurchasedFishData()
    {
        // Load the purchased fish data from PlayerPrefs or initialize if not present
        for (int i = 0; i < fishDataList.Count; i++)
        {
            purchasedFish[i] = PlayerPrefs.GetInt("Fish_" + i.ToString(), 0) == 1;
        }
    }

    public void OnPurchaseButtonClick()
    {
        int cost = fishDataList[selectedFishIndex].cost;

        if (GameManager.instance.ChangePearls(-cost))
        {
            purchasedFish[selectedFishIndex] = true;

            PlayerPrefs.SetInt("Fish_" + selectedFishIndex.ToString(), 1);
            PlayerPrefs.Save();

            UpdateShopUI();
        }
    }

    public void OnPlayButtonClick()
    {
        PlayerPrefs.SetInt("LastFish", selectedFishIndex);

        sceneHandler.ChangeScene(1);
    }

    private void UpdateShopUI()
    {
        // Update the buttons based on the purchased status
        if (purchasedFish[selectedFishIndex])
        {
            purchaseButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }
        else
        {
            purchaseButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);

            TextMeshProUGUI buyButtonText = purchaseButton.GetComponentInChildren<TextMeshProUGUI>();

            if (buyButtonText != null)
            {
                buyButtonText.text = "Pearls X" + fishDataList[selectedFishIndex].cost.ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on the Purchase Button.");
            }
        }

        pearlCounter.text = "Pearls: " + GameManager.instance.pearls.ToString() + "\vHighscore: " + PlayerPrefs.GetFloat("HighScore").ToString("F2");
    }
}
