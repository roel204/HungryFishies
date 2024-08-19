using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer foodSFX;
    public GameObject settingsMenuPanel;
    public Slider volumeSlider;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("FoodVolume", 0f);
        volumeSlider.value = savedVolume;

        if (savedVolume > -49)
        {
            foodSFX.SetFloat("FoodSfx", savedVolume);
        }
        else
        {
            foodSFX.SetFloat("FoodSfx", -80);
        }
    }

    public void OpenSettingsMenu()
    {
        settingsMenuPanel.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuPanel.SetActive(false);
    }

    public void UpdateVolume(float volume)
    {
        if (volume > -49)
        {
            foodSFX.SetFloat("FoodSfx", volume);
        }
        else
        {
            foodSFX.SetFloat("FoodSfx", -80);
        }
        PlayerPrefs.SetFloat("FoodVolume", volume);
        PlayerPrefs.Save();
    }
}