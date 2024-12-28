using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public Slider levelSlider;
    public Button upgradeButton;

    private Upgrade currentUpgrade;
    private System.Action onUpgradeClicked;

    public void Initialize(Upgrade upgrade, System.Action upgradeCallback)
    {
        currentUpgrade = upgrade;
        onUpgradeClicked = upgradeCallback;

        icon.sprite = upgrade.icon;
        nameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;
        levelSlider.maxValue = upgrade.maxLevel;
        UpdateUI(0);

        upgradeButton.onClick.AddListener(() => onUpgradeClicked.Invoke());
    }

    public void UpdateUI(int currentLevel)
    {
        levelSlider.value = currentLevel;
        priceText.text = "Cost: " + (currentUpgrade.baseCost + (currentLevel * currentUpgrade.priceIncrement)).ToString();
    }
}
