using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public GameObject audioSourcePrefab;
    public AudioSource eatAudioSource;

    public AudioMixer audioMixer;

    public AudioClip coin, pop, error, buy, hurt, boost, boostReverse;

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

    public void PlaySFXCoin()
    {
        PlaySound(coin);
    }

    public void PlaySFXEat()
    {
        eatAudioSource.Play();
    }

    public void PlaySFXError()
    {
        PlaySound(error);
    }

    public void PlaySFXBuy()
    {
        PlaySound(buy);
    }

    public void PlaySFXHurt()
    {
        PlaySound(hurt);
    }

    public void PlaySFXBoost()
    {
        PlaySound(boost);
    }

    public void PlaySFXBoostReverse()
    {
        PlaySound(boostReverse);
    }

    private void PlaySound(AudioClip clip)
    {
        // Create a new GameObject to hold the AudioSource component
        GameObject audioSourceObject = Instantiate(audioSourcePrefab, transform);

        // Get the AudioSource component from the new GameObject
        AudioSource src = audioSourceObject.GetComponent<AudioSource>();

        // Set the AudioClip and play the sound
        src.clip = clip;
        src.Play();

        // Destroy the temporary GameObject after the sound has finished playing
        Destroy(audioSourceObject, clip.length);
    }
}
