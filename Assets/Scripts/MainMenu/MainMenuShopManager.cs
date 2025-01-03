using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuShopManager : MonoBehaviour
{
    public Button purchaseButton;
    public Button playButton;
    public TextMeshProUGUI fishNameText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rotateText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI AbilityText;
    public GameObject scrollPanel;

    [SerializeField] private SimpleScrollSnap scrollSnap;
    private SceneHandler sceneHandler;

    private int selectedFishIndex = 0;
    private bool[] purchasedFish;

    private void Awake()
    {
        selectedFishIndex = PlayerPrefs.GetInt("LastFish");
    }

    private void Start()
    {
        sceneHandler = FindFirstObjectByType<SceneHandler>();

        purchasedFish = new bool[GameManager.instance.fishDataList.Count];
        LoadFish();

        UpdateShopUI();
    }

    public void SelectFish(int selectedItem, int previousItem)
    {
        selectedFishIndex = selectedItem;
        GameManager.instance.selectedFish = selectedFishIndex;

        UpdateShopUI();
    }

    private void LoadFish()
    {
        // Load the purchased fish data from PlayerPrefs or initialize if not present
        for (int i = 0; i < GameManager.instance.fishDataList.Count; i++)
        {
            // Check if the fish is purchased
            purchasedFish[i] = PlayerPrefs.GetInt("Unlocked_" + GameManager.instance.fishDataList[i].id, 0) == 1;
            purchasedFish[0] = true;

            // Add the panel using scrollSnap and access the newly added panel
            int childCountBefore = scrollSnap.Content.childCount;
            scrollSnap.Add(scrollPanel, i);

            // Get the Image component and assign the appropriate fish sprite
            GameObject newPanel = scrollSnap.Content.GetChild(childCountBefore).gameObject;

            Image fishImage = newPanel.GetComponent<Image>();
            fishImage.sprite = Resources.Load<Sprite>($"Sprites/Fishies/{GameManager.instance.fishDataList[i].id}");

            //Not working!
            //Animator animator = newPanel.GetComponent<Animator>();
            //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"Animations/Fishies/{GameManager.instance.fishDataList[i].id}");
        }

        // Set the starting panel and selected fish index
        scrollSnap.GoToPanel(selectedFishIndex);
        GameManager.instance.selectedFish = selectedFishIndex;
    }

    public void OnPurchaseButtonClick()
    {
        int cost = GameManager.instance.fishDataList[selectedFishIndex].cost;

        if (GameManager.instance.ChangeMoney(-cost))
        {
            purchasedFish[selectedFishIndex] = true;

            PlayerPrefs.SetInt("Unlocked_" + GameManager.instance.fishDataList[selectedFishIndex].id, 1);
            PlayerPrefs.Save();

            UpdateShopUI();

            SoundManager.Instance.PlaySound("Sfx", "buy");
        }
        else
        {
            SoundManager.Instance.PlaySound("Sfx", "error");
        }
    }

    public void OnPlayButtonClick()
    {
        PlayerPrefs.SetInt("LastFish", selectedFishIndex);

        SoundManager.Instance.PlaySound("Sfx", "pop");

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
                buyButtonText.text = GameManager.instance.fishDataList[selectedFishIndex].cost.ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on the Purchase Button.");
            }
        }

        fishNameText.text = GameManager.instance.fishDataList[selectedFishIndex].name.ToString();

        highScoreText.text = PlayerPrefs.GetFloat("HighScore_" + GameManager.instance.fishDataList[selectedFishIndex].id, 0).ToString("F2");

        speedText.text = GameManager.instance.fishDataList[selectedFishIndex].defaultSpeed.ToString();

        rotateText.text = GameManager.instance.fishDataList[selectedFishIndex].defaultRotate.ToString();

        HealthText.text = GameManager.instance.fishDataList[selectedFishIndex].defaultHealth.ToString();

        AbilityText.text = GameManager.instance.fishDataList[selectedFishIndex].abilityText;
    }
}
