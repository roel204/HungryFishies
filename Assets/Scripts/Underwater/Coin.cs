using UnityEngine;

public class Coin : MonoBehaviour
{
    private ObjectSpawner objectSpawner;
    private UpgradeManager UpgradeManager;

    private void Start()
    {
        objectSpawner = FindFirstObjectByType<ObjectSpawner>();
        UpgradeManager = FindFirstObjectByType<UpgradeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectSpawner != null)
        {
            if (collision.gameObject.CompareTag("Fish"))
            {
                if (UpgradeManager != null)
                {
                    UpgradeManager.IncreaseCoinCount(1f);
                    SoundManager.Instance.PlaySound("Sfx", "coin", 1f, 1.2f);
                }

                gameObject.SetActive(false);
                objectSpawner.StartCoinRespawnTimer(gameObject);
            }
        }
    }
}
