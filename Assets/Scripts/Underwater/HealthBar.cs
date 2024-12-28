using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI multiplier;
    private float baseHealth = 50;
    private float currentHealth;
    private float baseDecreaseSpeed = 8f;
    private float speedMultiplier;
    private float gameDuration;
    private bool isGameRunning;
    private UpgradeManager upgradeManager;
    private SceneHandler sceneHandler;

    private void Start()
    {
        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        sceneHandler = FindFirstObjectByType<SceneHandler>();

        baseHealth = GameManager.instance.fishDataList[GameManager.instance.selectedFish].defaultHealth;
        slider.maxValue = baseHealth;
        slider.value = baseHealth;
        currentHealth = baseHealth;
        isGameRunning = true;
        gameDuration = 0f;
        speedMultiplier = 1f;
        multiplier.text = "X" + speedMultiplier.ToString("F1");
    }

    private void Update()
    {
        if (isGameRunning)
        {
            slider.maxValue = baseHealth * (upgradeManager.GetLevel("FishHp") + 1);

            // Calculate the decrease speed multiplier based on the game duration
            gameDuration += Time.deltaTime;
            speedMultiplier = 1f + Mathf.Floor(gameDuration / 10f) * 0.1f;
            multiplier.text = "Hunger: X" + speedMultiplier.ToString("F1");

            // Decrease health over time based on the decreaseSpeed and speedMultiplier
            float decreaseAmount = baseDecreaseSpeed * speedMultiplier * Time.deltaTime;
            ChangeHealth(-decreaseAmount);

            if (currentHealth <= 0f)
            {
                SoundManager.Instance.PlaySound("Sfx", "hurt");
                GameManager.instance.GameOver();
                sceneHandler.ChangeScene(2);
                isGameRunning = false;
            }
        }
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, slider.maxValue);
        slider.value = currentHealth;
    }
}
