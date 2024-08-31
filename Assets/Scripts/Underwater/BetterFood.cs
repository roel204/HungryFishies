using UnityEngine;

public class BetterFood : MonoBehaviour
{
    public float healthIncreaseAmount = 20f;

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
                SFXManager.instance.PlaySfx("chomp", 1.5f, 2.5f, true);

                Instantiate(particlePrefab, transform.position, transform.rotation);

                gameObject.SetActive(false);
                objectSpawner.StartBetterFoodRespawnTimer(gameObject);
            }
        }
    }
}
