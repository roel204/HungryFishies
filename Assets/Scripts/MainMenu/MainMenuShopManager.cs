using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuShopManager : MonoBehaviour
{
    public int[] fishSkinCosts; // An array to store the cost of each fish skin

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

        LoadPurchasedFishData();
        UpdateShopUI();
    }

    public void SelectFish(int selectedItem, int previousItem)
    {
        selectedFishIndex = selectedItem;
        Debug.Log(selectedItem + "|" + previousItem);

        UpdateShopUI();
    }

    private void LoadPurchasedFishData()
    {
        // Load the purchased fish data from PlayerPrefs or initialize if not present
        purchasedFish = new bool[fishSkinCosts.Length];
        for (int i = 0; i < fishSkinCosts.Length; i++)
        {
            purchasedFish[i] = PlayerPrefs.GetInt("Fish_" + i.ToString(), 0) == 1;
        }
    }

    //private void SavePurchasedFishData()
    //{
    //    // Save the purchased fish data to PlayerPrefs
    //    for (int i = 0; i < fishSkinCosts.Length; i++)
    //    {
    //        PlayerPrefs.SetInt("Fish_" + i.ToString(), purchasedFish[i] ? 1 : 0);
    //    }
    //    PlayerPrefs.Save();
    //}

    public void OnPurchaseButtonClick()
    {
        int cost = fishSkinCosts[selectedFishIndex];

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
        GameManager.instance.selectedFish = selectedFishIndex;
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
                buyButtonText.text = "Pearls X" + fishSkinCosts[selectedFishIndex].ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on the Purchase Button.");
            }
        }

        pearlCounter.text = "Pearls: " + GameManager.instance.pearls.ToString() + "\vHighscore: " + PlayerPrefs.GetFloat("HighScore").ToString("F2");
    }
}
