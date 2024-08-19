using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float score;
    private int pearls;
    private Timer timer;
    [SerializeField] Animator transitionAnimator;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
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

    public void GameOver()
    {
        timer = FindObjectOfType<Timer>();
        score = timer.GetTimerValue();
    }

    public void AddPearls(int amount)
    {
        pearls = PlayerPrefs.GetInt("Pearls");
        pearls += amount;
        PlayerPrefs.SetInt("Pearls", pearls);
    }
}
