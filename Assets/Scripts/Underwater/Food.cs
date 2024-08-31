using UnityEngine;

public class Food : MonoBehaviour
{
    private readonly float healthIncreaseAmount = 15f; // Amount of health to increase when fish touches the food

    [SerializeField] private GameObject particlePrefab;

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
            if (healthBar != null && objectSpawner != null)
            {
                healthBar.ChangeHealth(healthIncreaseAmount);
                SFXManager.instance.PlaySFXEat();

                Instantiate(particlePrefab, transform.position, transform.rotation);

                gameObject.SetActive(false);
                objectSpawner.StartFoodRespawnTimer(gameObject);
            }
        }
    }

}
