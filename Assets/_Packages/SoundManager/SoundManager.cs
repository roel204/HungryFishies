using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    [Header("Audio Setup")]
    public AudioMixer audioMixer;
    public AudioMixerGroup[] audioMixerGroups;
    [SerializeField] private int initialPoolSize = 10;

    [Header("Audio Clips")]
    public AudioClip[] audioClips;

    private Dictionary<string, AudioClip> audioClipDictionary;
    private Dictionary<string, AudioMixerGroup> audioMixerGroupDictionary;
    private readonly List<GameObject> sceneUnloadStopList = new();
    private readonly Queue<GameObject> audioSourcePool = new();

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize dictionaries
        audioClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips) {
            if (clip != null && !audioClipDictionary.ContainsKey(clip.name)) {
                audioClipDictionary.Add(clip.name, clip);
            }
        }

        audioMixerGroupDictionary = new Dictionary<string, AudioMixerGroup>();
        foreach (var group in audioMixerGroups) {
            if (group != null && !audioMixerGroupDictionary.ContainsKey(group.name)) {
                audioMixerGroupDictionary.Add(group.name, group);
            }
        }

        InitializePool();
    }

    private void Start() {
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // Load saved volume for each Audio Group
        foreach (AudioMixerGroup group in audioMixerGroups) {
            if (group == null) continue;

            float savedVolume = PlayerPrefs.GetFloat($"{group.name}Volume", 69f);

            if (savedVolume == 69f) {
                audioMixer.GetFloat($"{group.name}Volume", out float currentVolume);
                PlayerPrefs.SetFloat($"{group.name}Volume", currentVolume);
            } else {
                audioMixer.SetFloat($"{group.name}Volume", savedVolume);
            }
        }
    }

    public void PlaySound(string audioMixerGroup, string clipName, float minPitch = 1, float maxPitch = 1, bool loop = false, float volume = 1f, int priority = 128, float fadeDuration = 0f, bool stopOnSceneUnload = false) {
        if (!audioClipDictionary.TryGetValue(clipName, out AudioClip newClip)) {
            Debug.LogWarning($"AudioClip with name '{clipName}' not found in SoundManager!");
            return;
        }

        if (!audioMixerGroupDictionary.TryGetValue(audioMixerGroup, out AudioMixerGroup targetGroup)) {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' not found in SoundManager!");
            return;
        }

        // Get a pooled AudioSource GameObject
        GameObject srcObj = GetPooledObject();
        srcObj.name = clipName;
        AudioSource src = srcObj.GetComponent<AudioSource>();

        // Configure the AudioSource
        src.clip = newClip;
        src.pitch = Random.Range(minPitch, maxPitch);
        src.outputAudioMixerGroup = targetGroup;
        src.loop = loop;
        src.volume = volume;
        src.priority = priority;

        if (stopOnSceneUnload) sceneUnloadStopList.Add(srcObj);

        if (fadeDuration > 0f) StartCoroutine(FadeVolume(src, 0f, volume, fadeDuration));

        src.Play();

        // Return to pool after length if not looping
        if (!loop) {
            StartCoroutine(ReturnToPool(srcObj, (newClip.length / src.pitch) + 0.1f));
        }
    }

    private IEnumerator FadeVolume(AudioSource source, float startVolume, float endVolume, float duration) {
        source.volume = startVolume;

        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        source.volume = endVolume;
    }

    // Stop methods:

    public void StopSoundsByName(string soundName, float fadeDuration = 0f) {
        if (!audioClipDictionary.TryGetValue(soundName, out AudioClip clip)) {
            Debug.LogWarning($"AudioClip with name '{soundName}' does not exist! No sounds stopped.");
            return;
        }

        // Find all AudioSources with the specified clip and stop them
        foreach (Transform child in transform) {
            AudioSource audioSource = child.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip == clip) {
                if (fadeDuration > 0f) StartCoroutine(FadeVolume(audioSource, audioSource.volume, 0f, fadeDuration));
                StartCoroutine(ReturnToPool(child.gameObject, fadeDuration + 0.1f));
                Debug.Log($"Stopped sound: {soundName}");
            }
        }
    }

    public void StopSoundsByGroup(string audioMixerGroup, float fadeDuration = 0f) {
        if (!audioMixerGroupDictionary.TryGetValue(audioMixerGroup, out AudioMixerGroup group)) {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' does not exist! No sounds stopped.");
            return;
        }

        // Find all AudioSources with the specified AudioMixerGroup and stop them
        foreach (Transform child in transform) {
            AudioSource audioSource = child.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.outputAudioMixerGroup == group) {
                if (fadeDuration > 0f) StartCoroutine(FadeVolume(audioSource, audioSource.volume, 0f, fadeDuration));
                StartCoroutine(ReturnToPool(child.gameObject, fadeDuration + 0.1f));
                Debug.Log($"Stopped sound from group: {audioMixerGroup}");
            }
        }
    }

    public void StopSoundByGameObject(GameObject gameObject, float fadeDuration = 0f) {
        if (gameObject == null) return;

        if (!gameObject.TryGetComponent<AudioSource>(out var audioSource)) {
            Debug.LogWarning("No AudioSource found on the provided GameObject. Nothing to stop.");
            return;
        }

        if (fadeDuration > 0f) StartCoroutine(FadeVolume(audioSource, audioSource.volume, 0f, fadeDuration));
        StartCoroutine(ReturnToPool(gameObject, fadeDuration + 0.1f));
        Debug.Log($"Stopped sound on GameObject: {gameObject.name}");
    }

    // Volume methods:

    // Set the volume for a AudioMixerGroup between -80 and 0
    public void SetVolume(string audioMixerGroup, float volume) {
        if (!audioMixerGroupDictionary.ContainsKey(audioMixerGroup)) {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' does not exist. Volume not set.");
            return;
        }

        volume = Mathf.Clamp(volume, -80f, 0f);

        PlayerPrefs.SetFloat($"{audioMixerGroup}Volume", volume);
        PlayerPrefs.Save();

        if (!audioMixer.SetFloat($"{audioMixerGroup}Volume", volume)) {
            Debug.LogWarning($"Failed to set volume for '{audioMixerGroup}'. Exposed parameter not found!");
        }
    }

    // Get the current volume for a AudioMixerGroup, returns 0 if not set or fails
    public float GetVolume(string audioMixerGroup) {
        if (!audioMixerGroupDictionary.ContainsKey(audioMixerGroup)) {
            Debug.LogWarning($"AudioMixerGroup '{audioMixerGroup}' does not exist. Returned 0");
            return 0f;
        }

        if (!audioMixer.GetFloat($"{audioMixerGroup}Volume", out float volume)) {
            Debug.LogWarning($"Failed to get volume for '{audioMixerGroup}' from the AudioMixer! Returned 0");
            return 0f;
        }

        return volume;
    }

    // Extra methods:

    public bool IsSoundPlaying(string soundName) {
        if (!audioClipDictionary.TryGetValue(soundName, out AudioClip clip)) {
            Debug.LogWarning($"AudioClip with name '{soundName}' does not exist! Cannot check if sound is playing.");
            return false;
        }

        // Check if any AudioSource is playing the specified clip
        foreach (Transform child in transform) {
            AudioSource audioSource = child.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip == clip && audioSource.isPlaying) {
                return true;
            }
        }
        return false;
    }

    private void OnSceneUnloaded(Scene arg0) {
        if (sceneUnloadStopList == null || sceneUnloadStopList.Count < 1) return;

        foreach (GameObject obj in sceneUnloadStopList) {
            StopSoundByGameObject(obj, 0.5f);
        }

        sceneUnloadStopList.Clear();
        Debug.Log("Stopped and destroyed all sounds in sceneUnloadStopList on scene unload.");
    }

    // Pool methods:

    private void InitializePool() {
        for (int i = 0; i < initialPoolSize; i++) {
            GameObject obj = new("PooledAudioSource");
            obj.transform.parent = transform;
            obj.SetActive(false);
            obj.AddComponent<AudioSource>();
            audioSourcePool.Enqueue(obj);
        }
    }

    private GameObject GetPooledObject() {
        if (audioSourcePool.Count > 0) {
            GameObject obj = audioSourcePool.Dequeue();
            obj.SetActive(true);
            return obj;
        } else {
            GameObject newObj = new("PooledAudioSource");
            newObj.transform.parent = transform;
            newObj.AddComponent<AudioSource>();
            return newObj;
        }
    }

    private IEnumerator ReturnToPool(GameObject obj, float delay) {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        obj.name = "PooledAudioSource";
        audioSourcePool.Enqueue(obj);
    }
}
