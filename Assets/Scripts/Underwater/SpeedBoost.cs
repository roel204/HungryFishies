using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpeedBoost : MonoBehaviour {

    public Fish fish;
    public Slider slider;
    public Button button;
    public InputActionProperty boostButton;

    private bool boostingAllowed = false;
    private bool boosting = false;
    private float energy = 3f;

    private void Start() {
        slider.maxValue = 10;
        slider.value = energy;

        if (GameManager.instance.fishDataList[GameManager.instance.selectedFish].abilities.Contains("boost")) {
            boostingAllowed = true;
            button.gameObject.SetActive(true); // Show the button

            boostButton.action.performed += ToggleSpeedBoost;
        } else {
            boostingAllowed = false;
            button.gameObject.SetActive(false); // Hide the button
        }
    }

    private void Update() {
        // If boosting, Boost the fish
        if (boosting) {
            if (energy > 0.1f) {
                float decreaseAmount = 3 * Time.deltaTime;
                ChangeEnergy(-decreaseAmount);
            } else {
                boosting = false;
                SoundManager.Instance.PlaySound("Sfx", "boostReverse");
            }
            fish.baseSpeed = GameManager.instance.fishDataList[GameManager.instance.selectedFish].defaultSpeed + 3;

        } else {
            float increaseAmount = 0.8f * Time.deltaTime;
            ChangeEnergy(increaseAmount);

            fish.baseSpeed = GameManager.instance.fishDataList[GameManager.instance.selectedFish].defaultSpeed;
        }
    }

    private void ChangeEnergy(float amount) {
        energy += amount;
        energy = Mathf.Clamp(energy, 0f, slider.maxValue);
        slider.value = energy;
    }

    private void ToggleSpeedBoost(InputAction.CallbackContext ctx) {
        if (boostingAllowed) {
            if (boosting) {
                boosting = false;
                SoundManager.Instance.PlaySound("Sfx", "boostReverse");
            } else if (energy > 2) {
                boosting = true;
                SoundManager.Instance.PlaySound("Sfx", "boost");
            } else {
                SoundManager.Instance.PlaySound("Sfx", "error");
            }
        }
    }

    private void OnDestroy() {
        boostButton.action.performed -= ToggleSpeedBoost;
    }
}
