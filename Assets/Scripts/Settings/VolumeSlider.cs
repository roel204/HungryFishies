using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerUpHandler
{
    public string audioMixerGroup;
    public string sfxOnRelease;

    private Slider volumeSlider;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        if (volumeSlider != null)
        {
            volumeSlider.minValue = -80;
            volumeSlider.maxValue = 0;
            volumeSlider.value = SoundManager.Instance.GetVolume(audioMixerGroup);
        }
    }

    public void ChangeVolume(float volume)
    {
        SoundManager.Instance.SetVolume(audioMixerGroup, volume);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(sfxOnRelease))
        {
            SoundManager.Instance.PlaySound(audioMixerGroup, sfxOnRelease);
        }
    }
}
