using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    [Header("Audio Mixer Settings")]
    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerGroupMusic;
    public AudioMixerGroup audioMixerGroupSfx;

    [Header("Audio Clips")]
    public AudioClip[] audioClips;

    public string playMusicOnStart = "";

    private Dictionary<string, AudioClip> audioClipDictionary;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        audioClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips) {
            if (clip != null && !audioClipDictionary.ContainsKey(clip.name)) {
                audioClipDictionary.Add(clip.name, clip);
            }
        }
    }

    private void Start() {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0f);

        audioMixer.SetFloat("MusicVolume", savedMusicVolume);
        audioMixer.SetFloat("SfxVolume", savedSfxVolume);

        if (playMusicOnStart != "") {
            PlayMusic(playMusicOnStart, 0f);
        }
    }

    public void PlaySfx(string clipName, float minPitch = 1, float maxPitch = 1, bool loop = false) {
        if (!audioClipDictionary.TryGetValue(clipName, out AudioClip newClip)) {
            Debug.LogWarning($"AudioClip with name '{clipName}' not found in SoundManager!");
            return;
        }

        // Create a new GameObject with an AudioSource
        GameObject sfxSourceObject = new GameObject($"SFX: {clipName}");
        sfxSourceObject.transform.parent = transform;
        AudioSource newSfxSrc = sfxSourceObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        newSfxSrc.clip = newClip;
        newSfxSrc.pitch = Random.Range(minPitch, maxPitch);
        newSfxSrc.outputAudioMixerGroup = audioMixerGroupSfx;
        newSfxSrc.loop = loop;

        // Play the sound and destroy after its length
        newSfxSrc.Play();

        if (!loop) {
            Destroy(sfxSourceObject, newClip.length / newSfxSrc.pitch);
        }
    }

    public void PlayMusic(string clipName, float fadeDuration = 1f, bool loop = true) {
        if (!audioClipDictionary.TryGetValue(clipName, out AudioClip newClip)) {
            Debug.LogWarning($"AudioClip with name '{clipName}' not found in SoundManager!");
            return;
        }

        // Create a new GameObject with an AudioSource
        GameObject musicSourceObject = new GameObject($"Music: {clipName}");
        musicSourceObject.transform.parent = transform;
        AudioSource newMusicSrc = musicSourceObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        newMusicSrc.clip = newClip;
        newMusicSrc.outputAudioMixerGroup = audioMixerGroupMusic;
        newMusicSrc.loop = loop;
        newMusicSrc.volume = 0f;

        // Play the music and fade in
        newMusicSrc.Play();
        StartCoroutine(FadeVolume(newMusicSrc, 0f, 1f, fadeDuration));

        if (!loop) {
            Destroy(musicSourceObject, newClip.length / newMusicSrc.pitch);
        }
    }

    private IEnumerator FadeVolume(AudioSource source, float startVolume, float endVolume, float duration) {
        float elapsed = 0f;

        source.volume = startVolume;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        source.volume = endVolume;
    }

    public void StopSounds(string soundName) {
        if (!audioClipDictionary.TryGetValue(soundName, out AudioClip clip)) {
            Debug.LogWarning($"AudioClip with name '{soundName}' not found in SoundManager!");
            return;
        }

        foreach (Transform child in transform) {
            AudioSource audioSource = child.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip == clip) {
                audioSource.Stop();
                Destroy(child.gameObject);
                Debug.Log($"Stopped and destroyed sound: {soundName}");
            }
        }
    }

    public void SetMusicVolume(float volume) {
        volume = Mathf.Clamp(volume, -80f, 0f);
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSfxVolume(float volume) {
        volume = Mathf.Clamp(volume, -80f, 0f);
        audioMixer.SetFloat("SfxVolume", volume);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public float GetMusicVolume() {
        if (audioMixer.GetFloat("MusicVolume", out float volume)) {
            return volume;
        } else {
            Debug.LogWarning("Failed to get MusicVolume from the AudioMixer!");
            return -80f;
        }
    }

    public float GetSfxVolume() {
        if (audioMixer.GetFloat("SfxVolume", out float volume)) {
            return volume;
        } else {
            Debug.LogWarning("Failed to get SfxVolume from the AudioMixer!");
            return -80f;
        }
    }
}
