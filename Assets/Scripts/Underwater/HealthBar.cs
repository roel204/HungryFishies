using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI multiplier;
    private float baseHealth = GameManager.instance.fishDataList[GameManager.instance.selectedFish].defaultHealth;
    private float currentHealth;
    private float baseDecreaseSpeed = 8f;
    private float speedMultiplier;
    private float gameDuration;
    private bool isGameRunning;
    private ShopManager shopManager;
    private SceneHandler sceneHandler;

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        sceneHandler = FindFirstObjectByType<SceneHandler>();

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
            slider.maxValue = baseHealth * (shopManager.shopItems[3, 6] + 1);

            // Calculate the decrease speed multiplier based on the game duration
            gameDuration += Time.deltaTime;
            speedMultiplier = 1f + Mathf.Floor(gameDuration / 10f) * 0.1f;
            multiplier.text = "Hunger: X" + speedMultiplier.ToString("F1");

            // Decrease health over time based on the decreaseSpeed and speedMultiplier
            float decreaseAmount = baseDecreaseSpeed * speedMultiplier * Time.deltaTime;
            DecreaseHealth(decreaseAmount);

            if (currentHealth <= 0f)
            {
                GameManager.instance.GameOver();
                sceneHandler.ChangeScene(2);
                isGameRunning = false;
            }
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, slider.maxValue);
        slider.value = currentHealth;
    }

    private void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, slider.maxValue);
        slider.value = currentHealth;
    }
}
