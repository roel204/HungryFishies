using UnityEngine;

public class BetterFood : MonoBehaviour
{
    public float healthIncreaseAmount = 20f;

    private ObjectSpawner objectSpawner;
    private SFXManager sfxManager;
    private HealthBar healthBar;

    private void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        sfxManager = FindObjectOfType<SFXManager>();
        healthBar = FindObjectOfType<HealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            if (healthBar != null)
            {
                healthBar.AddHealth(healthIncreaseAmount);
                sfxManager.PlaySFXPop();
            }

            gameObject.SetActive(false);
            objectSpawner.StartBetterFoodRespawnTimer(gameObject);
        }
    }
}
