using UnityEngine;

public class Food : MonoBehaviour
{
    public float healthIncreaseAmount = 10f; // Amount of health to increase when fish touches the food

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
        if (objectSpawner != null)
        {
            if (collision.gameObject.CompareTag("Fish"))
            {
                if (healthBar != null)
                {
                    healthBar.AddHealth(healthIncreaseAmount);
                    sfxManager.PlaySFXPop();
                }

                gameObject.SetActive(false);
                objectSpawner.StartFoodRespawnTimer(gameObject);
            }
        }
    }
}
