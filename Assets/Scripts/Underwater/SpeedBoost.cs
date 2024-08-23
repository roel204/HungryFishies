using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoost : MonoBehaviour
{

    public Fish fish;
    public Slider slider;
    public Button button;

    private bool boosting = false;
    private float energy = 3f;

    private void Start()
    {
        slider.maxValue = 10;
        slider.value = energy;

        if (GameManager.instance.selectedFish == 1 || GameManager.instance.selectedFish == 3 || GameManager.instance.selectedFish == 8 || GameManager.instance.selectedFish == 11)
        {
            button.gameObject.SetActive(true); // Show the button
        }
        else
        {
            button.gameObject.SetActive(false); // Hide the button
        }
    }

    private void Update()
    {
        if (boosting)
        {
            if (energy > 0.1f)
            {
                float decreaseAmount = 3 * Time.deltaTime;
                ChangeEnergy(-decreaseAmount);
            }
            else
            {
                boosting = false;
            }
            fish.baseSpeed = GameManager.instance.fishDataList[GameManager.instance.selectedFish].defaultSpeed + 3;

        }
        else
        {
            float increaseAmount = 0.8f * Time.deltaTime;
            ChangeEnergy(increaseAmount);

            fish.baseSpeed = GameManager.instance.fishDataList[GameManager.instance.selectedFish].defaultSpeed;
        }
    }

    private void ChangeEnergy(float amount)
    {
        energy += amount;
        energy = Mathf.Clamp(energy, 0f, slider.maxValue);
        slider.value = energy;
    }

    public void ToggleSpeedBoost()
    {
        boosting = !boosting;
    }

}
