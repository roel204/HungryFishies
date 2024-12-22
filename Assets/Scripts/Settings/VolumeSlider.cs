using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public string audioMixerGroup;
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = SoundManager.Instance.GetVolume(audioMixerGroup);
    }

    public void ChangeVolume(float volume)
    {
        SoundManager.Instance.SetVolume(audioMixerGroup, volume);
    }
}
