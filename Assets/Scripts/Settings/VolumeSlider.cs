using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerUpHandler
{
    public string audioMixerGroup;
    public string sfxOnRelease;

    private Slider volumeSlider;
    private int lastPlayedValue = 9999;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();

        if (volumeSlider == null)
        {
            Debug.LogError("No Slider component found on this GameObject!");
        }
    }

    private void Start()
    {
        if (volumeSlider != null)
        {
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
