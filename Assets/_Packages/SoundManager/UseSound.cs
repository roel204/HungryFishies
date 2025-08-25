using UnityEngine;

public class UseSound : MonoBehaviour {

    [Header("Sound Setup")]
    [SerializeField] private string audioMixerGroup;
    [SerializeField] private string clipName;
    [SerializeField] private float minPitch = 1;
    [SerializeField] private float maxPitch = 1;
    [SerializeField] private bool loop = false;
    [SerializeField] private float volume = 1;
    [SerializeField] private int priority = 128;
    [SerializeField] private float fadeInDuration = 0f;
    [SerializeField] private bool stopOnSceneUnload = false;

    [Header("Extra")]
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool preventDuplicates = false;
    [SerializeField] private bool stopByNameOnStart = false;
    [SerializeField] private bool stopByGroupOnStart = false;
    [SerializeField] private float fadeOutDuration = 0f;

    private void Start() {
        if (playOnStart) PlaySound();
        if (stopByNameOnStart) StopSoundsByName(clipName);
        if (stopByGroupOnStart) StopSoundsByGroup(audioMixerGroup);
    }

    public void PlaySound() {
        if (preventDuplicates && SoundManager.Instance.IsSoundPlaying(clipName)) return;
        SoundManager.Instance.PlaySound(audioMixerGroup, clipName, minPitch, maxPitch, loop, volume, priority, fadeInDuration, stopOnSceneUnload);
    }

    public void StopSoundsByName(string clipName) {
        SoundManager.Instance.StopSoundsByName(clipName, fadeOutDuration);
    }

    public void StopSoundsByGroup(string audioMixerGroup) {
        SoundManager.Instance.StopSoundsByGroup(audioMixerGroup, fadeOutDuration);
    }
}
