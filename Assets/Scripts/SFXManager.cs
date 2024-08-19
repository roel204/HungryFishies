using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public GameObject audioSourcePrefab;
    public AudioSource audioSource;
    public AudioMixer foodSFX;

    public AudioClip coin, pop, error, buy;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("FoodVolume", 0f);
        if (savedVolume > -49)
        {
            foodSFX.SetFloat("FoodSfx", savedVolume);
        }
        else
        {
            foodSFX.SetFloat("FoodSfx", -80);
        }
    }

    public void PlaySFXCoin()
    {
        PlaySound(coin);
    }

    public void PlaySFXPop()
    {
        audioSource.Play();
    }

    public void PlaySFXError()
    {
        PlaySound(error);
    }

    public void PlaySFXBuy()
    {
        PlaySound(buy);
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
