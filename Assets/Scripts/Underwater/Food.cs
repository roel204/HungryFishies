using UnityEngine;

public class Food : MonoBehaviour
{
    private readonly float healthIncreaseAmount = 15f; // Amount of health to increase when fish touches the food

    [SerializeField] private GameObject particlePrefab;

    private ObjectSpawner objectSpawner;
    private HealthBar healthBar;

    private void Start()
    {
        objectSpawner = FindFirstObjectByType<ObjectSpawner>();
        healthBar = FindFirstObjectByType<HealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            if (healthBar != null && objectSpawner != null)
            {
                healthBar.ChangeHealth(healthIncreaseAmount);
                SoundManager.Instance.PlaySound("Eat", "chomp", 2f, 3f);

                Instantiate(particlePrefab, transform.position, transform.rotation);

                gameObject.SetActive(false);
                objectSpawner.StartFoodRespawnTimer(gameObject);
            }
        }
    }

}
