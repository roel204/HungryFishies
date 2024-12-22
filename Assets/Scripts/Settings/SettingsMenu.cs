using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsMenuPanel;
    public Toggle autoSwimToggle;
    public Toggle steeringButtonsToggle;

    private void Start()
    {
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