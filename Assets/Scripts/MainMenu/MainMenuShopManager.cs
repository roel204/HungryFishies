using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuShopManager : MonoBehaviour
{
    public Button purchaseButton;
    public Button playButton;
    public TextMeshProUGUI pearlCounter;
    public TextMeshProUGUI fishNameText;
    public TextMeshProUGUI highScoreText;
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

        // Initialize the purchased fish array based on the size of fishDataList
        purchasedFish = new bool[GameManager.instance.fishDataList.Count];

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
        for (int i = 0; i < GameManager.instance.fishDataList.Count; i++)
        {
            purchasedFish[i] = PlayerPrefs.GetInt("UnlockedFish_" + i.ToString(), 0) == 1;
        }
    }

    public void OnPurchaseButtonClick()
    {
        int cost = GameManager.instance.fishDataList[selectedFishIndex].cost;

        if (GameManager.instance.ChangePearls(-cost))
        {
            purchasedFish[selectedFishIndex] = true;

            PlayerPrefs.SetInt("UnlockedFish_" + selectedFishIndex.ToString(), 1);
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
                buyButtonText.text = "Pearls X" + GameManager.instance.fishDataList[selectedFishIndex].cost.ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on the Purchase Button.");
            }
        }

        fishNameText.text = GameManager.instance.fishDataList[selectedFishIndex].name.ToString();

        highScoreText.text = "HighScore: " + PlayerPrefs.GetFloat("HighScoreFish_" + selectedFishIndex, 0).ToString("F2");

        pearlCounter.text = "Pearls: " + GameManager.instance.pearls.ToString() + "\nHighscore: " + PlayerPrefs.GetFloat("HighScore").ToString("F2");
    }
}
