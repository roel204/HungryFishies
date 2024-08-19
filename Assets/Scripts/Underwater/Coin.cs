using UnityEngine;

public class Coin : MonoBehaviour
{
    private ObjectSpawner objectSpawner;
    private SFXManager sfxManager;
    private ShopManager shopManager;

    private void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        sfxManager = FindObjectOfType<SFXManager>();
        shopManager = FindObjectOfType<ShopManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectSpawner != null)
        {
            if (collision.gameObject.CompareTag("Fish"))
            {
                if (shopManager != null)
                {
                    shopManager.IncreaseCoinCount(1f);
                    sfxManager.PlaySFXCoin();
                }

                gameObject.SetActive(false);
                objectSpawner.StartCoinRespawnTimer(gameObject);
            }
        }
    }
}
