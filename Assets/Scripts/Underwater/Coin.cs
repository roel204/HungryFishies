using UnityEngine;

public class Coin : MonoBehaviour
{
    private ObjectSpawner objectSpawner;
    private ShopManager shopManager;

    private void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
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
                    SFXManager.instance.PlaySFXCoin();
                }

                gameObject.SetActive(false);
                objectSpawner.StartCoinRespawnTimer(gameObject);
            }
        }
    }
}
