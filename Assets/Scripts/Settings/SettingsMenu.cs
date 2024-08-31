using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsMenuPanel;
    public Slider eatVolumeSlider;
    public Slider generalVolumeSlider;
    public Toggle autoSwimToggle;
    public Toggle steeringButtonsToggle;

    private void Start()
    {
        float savedEatVolume = PlayerPrefs.GetFloat("FoodVolume", 0f);
        eatVolumeSlider.value = savedEatVolume;

        if (savedEatVolume > -49)
        {
            audioMixer.SetFloat("FoodSfx", savedEatVolume);
        }
        else
        {
            audioMixer.SetFloat("FoodSfx", -80);
        }

        float savedGeneralVolume = PlayerPrefs.GetFloat("FoodVolume", 0f);
        generalVolumeSlider.value = savedGeneralVolume;

        if (savedGeneralVolume > -49)
        {
            audioMixer.SetFloat("GeneralSfx", savedGeneralVolume);
        }
        else
        {
            audioMixer.SetFloat("GeneralSfx", -80);
        }

        autoSwimToggle.isOn = PlayerPrefs.GetInt("AutoSwim", 0) == 1;
        steeringButtonsToggle.isOn = PlayerPrefs.GetInt("lrTurn", 0) == 1;
    }

    public void OpenSettingsMenu()
    {
        settingsMenuPanel.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuPanel.SetActive(false);
    }

    public void UpdateEatVolume(float volume)
    {
        if (volume > -49)
        {
            audioMixer.SetFloat("FoodSfx", volume);
        }
        else
        {
            audioMixer.SetFloat("FoodSfx", -80);
        }
        PlayerPrefs.SetFloat("FoodVolume", volume);
        PlayerPrefs.Save();
    }

    public void UpdateGeneralVolume(float volume)
    {
        if (volume > -49)
        {
            audioMixer.SetFloat("GeneralSfx", volume);
        }
        else
        {
            audioMixer.SetFloat("GeneralSfx", -80);
        }
        PlayerPrefs.SetFloat("GeneralVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnAutoSwimToggleChanged()
    {
        if (steeringButtonsToggle.isOn && !autoSwimToggle.isOn)
        {
            steeringButtonsToggle.isOn = false;
        }
        PlayerPrefs.SetInt("lrTurn", steeringButtonsToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("AutoSwim", autoSwimToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnSteeringButtonsToggleChanged()
    {
        if (steeringButtonsToggle.isOn)
        {
            autoSwimToggle.isOn = true;
        }
        PlayerPrefs.SetInt("lrTurn", steeringButtonsToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("AutoSwim", autoSwimToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}