using UnityEngine;

public class Fish : MonoBehaviour
{
    public Sprite[] fishSkins;
    public bool followMouse = false;
    private KeyCode mouseButton = KeyCode.Mouse0;

    private float currentSpeed;
    private float baseSpeed = 3f;
    private float speedIncreasePerLevel = 1f;

    private float currentScale;
    private float baseScale = 1f;
    private float scaleIncreasePerLevel = 0.2f;

    private Vector3 targetPosition;

    private ShopManager shopManager;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentSpeed = baseSpeed;
        currentScale = baseScale;

        shopManager = FindObjectOfType<ShopManager>();

        int activeFishIndex = PlayerPrefs.GetInt("ActiveFishIndex", 0);
        GetComponent<SpriteRenderer>().sprite = fishSkins[activeFishIndex];
    }

    private void Update()
    {
        if (Input.GetKey(mouseButton))
        {
            followMouse = true;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            targetPosition = mousePosition;
            MoveAndRotateFish();
        }
        else
        {
            followMouse = false;
        }


        // Update the swimming speed based on the level of the first item in the shop
        int speedItemLevel = shopManager.shopItems[3, 1];
        currentSpeed = baseSpeed + (speedItemLevel * speedIncreasePerLevel);

        // Update the scale based on the level of the third item in the shop
        int scaleItemLevel = shopManager.shopItems[3, 3];
        currentScale = baseScale + (scaleItemLevel * scaleIncreasePerLevel);
    }

    private void MoveAndRotateFish()
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly move the fish towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // Rotate and flip the sprite based on the fish's movement direction
        if (direction.x >= 0)
        {
            transform.localScale = new Vector3(-currentScale, currentScale, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-currentScale, -currentScale, 1f);
        }

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

}