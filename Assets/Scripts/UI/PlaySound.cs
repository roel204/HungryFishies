using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private string audioMixerGroup;

    [SerializeField]
    private string sound;

    [SerializeField]
    private float minPitch = 1;

    [SerializeField]
    private float maxPitch = 1;

    [SerializeField]
    private bool loop = false;

    [SerializeField]
    private float volume = 1;

    public void PlayAudio()
    {
        SoundManager.Instance.PlaySound(audioMixerGroup, sound, minPitch, maxPitch, loop, volume);
    }
}
