using UnityEngine;

public class BetterFood : MonoBehaviour
{
    public float healthIncreaseAmount = 20f;

    private ObjectSpawner objectSpawner;
    private HealthBar healthBar;

    private void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        healthBar = FindObjectOfType<HealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            if (healthBar != null)
            {
                healthBar.ChangeHealth(healthIncreaseAmount);
                SFXManager.instance.PlaySFXEat();
            }

            gameObject.SetActive(false);
            objectSpawner.StartBetterFoodRespawnTimer(gameObject);
        }
    }
}
