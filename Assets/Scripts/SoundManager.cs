using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer audioMixer;
    public AudioMixerGroup[] audioMixerGroups;

    public AudioClip[] audioClips;

    public string playMusicOnStart = "";

    private Dictionary<string, AudioClip> audioClipDictionary;
    private Dictionary<string, AudioMixerGroup> audioMixerGroupDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize dictionaries
        audioClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
        {
            if (clip != null && !audioClipDictionary.ContainsKey(clip.name))
            {
                audioClipDictionary.Add(clip.name, clip);
            }
        }

        audioMixerGroupDictionary = new Dictionary<string, AudioMixerGroup>();
        foreach (var group in audioMixerGroups)
        {
            if (group != null && !audioMixerGroupDictionary.ContainsKey(group.name))
            {
                audioMixerGroupDictionary.Add(group.name, group);
            }
        }
    }

    private void Start()
    {
        // Load saved volume for each Audio Group
        foreach (AudioMixerGroup group in audioMixerGroups)
        {
            if (group == null) continue;

            float savedVolume = PlayerPrefs.GetFloat($"{group.name}Volume", 0f);
            audioMixer.SetFloat($"{group.name}Volume", savedVolume);

        }

        if (playMusicOnStart != "")
        {
            PlaySound("Music", playMusicOnStart, loop:true);
        }
    }

    public void PlaySound(string audioMixerGroup, string clipName, float minPitch = 1, float maxPitch = 1, bool loop = false, float volume = 1f)
    {
        if (!audioClipDictionary.TryGetValue(clipName, out AudioClip newClip))
        {
            Debug.LogWarning($"AudioClip with name '{clipName}' not found in SoundManager!");
            return;
        }

        if (!audioMixerGroupDictionary.TryGetValue(audioMixerGroup, out AudioMixerGroup targetGroup))
        {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' not found in SoundManager!");
            return;
        }

        // Create a new GameObject with an AudioSource
        GameObject sfxSourceObject = new($"{clipName}");
        sfxSourceObject.transform.parent = transform;
        AudioSource src = sfxSourceObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        src.clip = newClip;
        src.pitch = Random.Range(minPitch, maxPitch);
        src.outputAudioMixerGroup = targetGroup; // Assign the correct AudioMixerGroup
        src.loop = loop;
        src.volume = volume;

        // Play the sound and destroy after its length
        src.Play();

        if (!loop)
        {
            Destroy(sfxSourceObject, newClip.length / src.pitch);
        }
    }

    private IEnumerator FadeVolume(AudioSource source, float startVolume, float endVolume, float duration)
    {
        float elapsed = 0f;

        source.volume = startVolume;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        source.volume = endVolume;
    }

    public void StopSounds(string soundName)
    {
        if (!audioClipDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            Debug.LogWarning($"AudioClip with name '{soundName}' not found in SoundManager!");
            return;
        }

        foreach (Transform child in transform)
        {
            AudioSource audioSource = child.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip == clip)
            {
                audioSource.Stop();
                Destroy(child.gameObject);
                Debug.Log($"Stopped and destroyed sound: {soundName}");
            }
        }
    }

    public void SetVolume(string audioMixerGroup, float volume)
    {
        if (!audioMixerGroupDictionary.ContainsKey(audioMixerGroup))
        {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' does not exist. Volume not set.");
            return;
        }

        volume = Mathf.Clamp(volume, -80f, 0f);
        audioMixer.SetFloat($"{audioMixerGroup}Volume", volume);
        PlayerPrefs.SetFloat($"{audioMixerGroup}Volume", volume);
    }

    public float GetVolume(string audioMixerGroup)
    {
        if (!audioMixerGroupDictionary.ContainsKey(audioMixerGroup))
        {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' does not exist. Volume not set.");
            return -80f;
        }

        if (audioMixer.GetFloat($"{audioMixerGroup}Volume", out float volume))
        {
            return volume;
        }
        else
        {
            Debug.LogWarning($"Failed to get volume for '{audioMixerGroup}' from the AudioMixer!");
            return -80f;
        }
    }
}
