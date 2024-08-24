using System.Linq;
using UnityEngine;

public class JellyfishMovement : MonoBehaviour
{
    private readonly float moveSpeed = 1f;
    private readonly float changeDirectionTime = 5f;
    private readonly float minX = -18f;
    private readonly float maxX = 18f;
    private readonly float minY = -9f;
    private readonly float maxY = 9f;

    private Vector3 targetPosition;
    private float timer;

    private readonly float healthDecreaseAmount = 10f;

    private ObjectSpawner objectSpawner;
    private HealthBar healthBar;

    private void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        healthBar = FindObjectOfType<HealthBar>();

        SetNewRandomTargetPosition();
    }

    private void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update the timer
        timer += Time.deltaTime;

        // If the jellyfish reaches the target or the timer exceeds the changeDirectionTime, set a new target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || timer >= changeDirectionTime)
        {
            SetNewRandomTargetPosition();
            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            if (GameManager.instance.fishDataList[GameManager.instance.selectedFish].abilities.Contains("eatJelly"))
            {
                healthBar.ChangeHealth(healthDecreaseAmount);
                gameObject.SetActive(false);
                objectSpawner.StartJellyRespawnTimer(gameObject);
            }
            else
            {
                healthBar.ChangeHealth(-healthDecreaseAmount);
            }
        }
    }

    private void SetNewRandomTargetPosition()
    {
        // Generate a random position within the specified bounds
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        targetPosition = new Vector3(randomX, randomY, transform.position.z);
    }
}
