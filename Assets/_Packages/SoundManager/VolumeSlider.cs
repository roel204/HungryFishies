using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour, IPointerUpHandler {

    public string audioMixerGroup;
    public string sfxOnRelease;
    public bool isLogarithmic = true;
    public float logarithmicPower = 3f;

    private Slider volumeSlider;

    private void Start() {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        // Get saved dB value and convert it to slider position
        float savedDb = PlayerPrefs.GetFloat($"{audioMixerGroup}Volume", 69f);
        if (savedDb == 69f) savedDb = SoundManager.Instance.GetVolume(audioMixerGroup);

        volumeSlider.value = VolumeToSlider(savedDb);
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float sliderValue) {
        float volumeDb = SliderToVolume(sliderValue);
        SoundManager.Instance.SetVolume(audioMixerGroup, volumeDb);
        PlayerPrefs.SetFloat($"{audioMixerGroup}Volume", volumeDb);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (!string.IsNullOrEmpty(sfxOnRelease)) SoundManager.Instance.PlaySound(audioMixerGroup, sfxOnRelease);
    }

    private void OnDestroy() {
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    // Converts from slider [0, 1] to dB [-80, 0] with logarithmic curve
    private float SliderToVolume(float sliderValue) {
        if (!isLogarithmic) return Mathf.Lerp(-80f, 0f, sliderValue);

        sliderValue = Mathf.Clamp01(sliderValue);
        float curved = Mathf.Pow(sliderValue, 1f / logarithmicPower);
        return Mathf.Lerp(-80f, 0f, curved);
    }

    // Converts from dB [-80, 0] to slider [0, 1] using inverse of above
    private float VolumeToSlider(float volumeDb) {
        if (!isLogarithmic) return Mathf.InverseLerp(-80f, 0f, volumeDb);

        float linear = Mathf.InverseLerp(-80f, 0f, volumeDb);
        return Mathf.Pow(linear, logarithmicPower);
    }
}
