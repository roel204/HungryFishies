using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public GameObject audioSourcePrefab;

    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerGroupEat;
    public AudioMixerGroup audioMixerGroupSfx;

    public AudioClip coin, pop, chomp, error, buy, hurt, boost, boostReverse;

    private Dictionary<string, AudioClip> audioClips;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SFXManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the dictionary with audio clips
        audioClips = new Dictionary<string, AudioClip>
        {
            { "coin", coin },
            { "pop", pop },
            { "chomp", chomp },
            { "error", error },
            { "buy", buy },
            { "hurt", hurt },
            { "boost", boost },
            { "boostReverse", boostReverse }
        };
    }

    private void Start()
    {
        // Load and set the saved volumes for food and general SFX
        float savedFoodVolume = PlayerPrefs.GetFloat("FoodVolume", 0f);
        float savedGeneralVolume = PlayerPrefs.GetFloat("GeneralVolume", 0f);

        if (savedFoodVolume > -49)
        {
            audioMixer.SetFloat("FoodSfx", savedFoodVolume);
        }
        else
        {
            audioMixer.SetFloat("FoodSfx", -80);
        }

        if (savedGeneralVolume > -49)
        {
            audioMixer.SetFloat("GeneralSfx", savedGeneralVolume);
        }
        else
        {
            audioMixer.SetFloat("GeneralSfx", -80);
        }
    }

    public void PlaySfx(string clipName, float minPitch = 1, float maxPitch = 1, bool food = false)
    {
        // Check if the clip exists in the dictionary
        if (!audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            Debug.LogWarning($"AudioClip with name {clipName} not found!");
            return;
        }

        // Create a new GameObject to hold the AudioSource component
        GameObject audioSourceObject = Instantiate(audioSourcePrefab, transform);

        // Get the AudioSource component from the new GameObject
        AudioSource src = audioSourceObject.GetComponent<AudioSource>();

        // Set the AudioClip and play the sound
        src.clip = clip;

        // Set the appropriate AudioMixerGroup
        if (food)
        {
            src.outputAudioMixerGroup = audioMixerGroupEat;
        }
        else
        {
            src.outputAudioMixerGroup = audioMixerGroupSfx;
        }

        src.pitch = Random.Range(minPitch, maxPitch);

        src.Play();

        // Destroy the temporary GameObject after the sound has finished playing
        Destroy(audioSourceObject, clip.length);
    }
}
