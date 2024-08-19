using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float score;
    public int pearls;
    public int selectedFish;
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

        pearls = PlayerPrefs.GetInt("Pearls");
    }

    private void Start()
    {
        pearls = PlayerPrefs.GetInt("Pearls");
    }

    public void GameOver()
    {
        timer = FindObjectOfType<Timer>();
        score = timer.GetTimerValue();
    }

    public bool ChangePearls(int amount)
    {
        if (pearls + amount >= 0)
        {
            pearls += amount;
            PlayerPrefs.SetInt("Pearls", pearls);

            //sfxManager.PlaySFXBuy();

            return true;
        }
        else
        {
            //sfxManager.PlaySFXError();

            return false;
        }
    }
}
