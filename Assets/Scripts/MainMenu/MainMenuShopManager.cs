using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuShopManager : MonoBehaviour
{
    public Sprite[] fishSkins; // An array of all available fish skins to display
    public int[] fishSkinCosts; // An array to store the cost of each fish skin
    public Button purchaseButton;
    public Button playButton;
    public TextMeshProUGUI pearlCounter;
    public Image selectedFishImage; // Reference to the UI Image to display the selected fish
    private SFXManager sfxManager;
    private SceneHandler sceneHandler;

    private int selectedFishIndex = 0;
    private bool[] purchasedFish; // An array to store purchased fish data

    private void Start()
    {
        sfxManager = FindObjectOfType<SFXManager>();
        sceneHandler = FindFirstObjectByType<SceneHandler>();


        pearlCounter.text = "Pearls: " + PlayerPrefs.GetInt("Pearls").ToString() + "\vHighscore: " + PlayerPrefs.GetFloat("HighScore").ToString("F2");

        selectedFishIndex = PlayerPrefs.GetInt("ActiveFishIndex");

        LoadPurchasedFishData();
        UpdateShopUI();
    }

    private void LoadPurchasedFishData()
    {
        // Load the purchased fish data from PlayerPrefs or initialize if not present
        purchasedFish = new bool[fishSkins.Length];
        for (int i = 0; i < fishSkins.Length; i++)
        {
            purchasedFish[i] = PlayerPrefs.GetInt("Fish_" + i.ToString(), 0) == 1;
        }
    }

    private void SavePurchasedFishData()
    {
        // Save the purchased fish data to PlayerPrefs
        for (int i = 0; i < fishSkins.Length; i++)
        {
            PlayerPrefs.SetInt("Fish_" + i.ToString(), purchasedFish[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void UpdateShopUI()
    {
        selectedFishImage.sprite = fishSkins[selectedFishIndex];

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
        }

        TextMeshProUGUI buttonText = purchaseButton.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            buttonText.text = "Pearls X" + fishSkinCosts[selectedFishIndex].ToString();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found on the Purchase Button.");
        }
    }

    public void OnLeftArrowClick()
    {
        selectedFishIndex = (selectedFishIndex - 1 + fishSkins.Length) % fishSkins.Length;
        UpdateShopUI();
    }

    public void OnRightArrowClick()
    {
        selectedFishIndex = (selectedFishIndex + 1) % fishSkins.Length;
        UpdateShopUI();
    }

    public void OnPurchaseButtonClick()
    {
        int cost = fishSkinCosts[selectedFishIndex];
        int pearls = PlayerPrefs.GetInt("Pearls", 0);

        if (pearls >= cost)
        {
            GameManager.instance.AddPearls(-cost);
            sfxManager.PlaySFXBuy();

            purchasedFish[selectedFishIndex] = true;
            SavePurchasedFishData();

            UpdateShopUI();
            pearlCounter.text = "Pearls: " + PlayerPrefs.GetInt("Pearls").ToString() + "\vHighscore: " + PlayerPrefs.GetFloat("HighScore").ToString("F2");
        }
        else
        {
            sfxManager.PlaySFXError();
        }
    }


    public void OnPlayButtonClick()
    {
        int activeFishIndex = selectedFishIndex;
        PlayerPrefs.SetInt("ActiveFishIndex", activeFishIndex);
        PlayerPrefs.Save();

        sceneHandler.ChangeScene(1);
    }
}
