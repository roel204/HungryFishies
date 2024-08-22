using TMPro.Examples;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public bool followMouse = false;
    private KeyCode mouseButton = KeyCode.Mouse0;

    private float currentSpeed;
    private float baseSpeed;
    private float speedIncreasePerLevel = 1f;

    private float currentScale;
    private float baseScale = 1f;
    private float scaleIncreasePerLevel = 0.2f;

    private float currentRotateSpeed;
    private float baseRotateSpeed;
    private float rotateSpeedIncreasePerLevel = 40f;

    private Vector3 targetPosition = new(10, 10, 0);
    private ShopManager shopManager;

    private void Start()
    {
        // Get selected fish data
        FishData selectedFishData = GameManager.instance.fishDataList[GameManager.instance.selectedFish];

        // Initialize the speed and rotation based on the selected fish's data
        baseSpeed = selectedFishData.defaultSpeed;
        baseRotateSpeed = selectedFishData.defaultRotate;

        currentSpeed = baseSpeed;
        currentScale = baseScale;
        currentRotateSpeed = baseRotateSpeed;
        // Set the sprite based on the selected fish's name
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/Fishies/{selectedFishData.index}");

        // Set the BoxCollider2D size to match the sprite's size
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            // Match the collider size to the sprite's bounds
            boxCollider.size = spriteRenderer.bounds.size;
            boxCollider.offset = spriteRenderer.bounds.center - transform.position;
        }

        shopManager = FindObjectOfType<ShopManager>();

        MoveAndRotateFish();
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

        // Speed Increase for Ray
        if (GameManager.instance.selectedFish == 7)
        {
            // Increase speed as the fish's Y position drops below a certain threshold
            float topY = -6f;
            float bottomY = -10f;

            if (transform.position.y < topY)
            {
                // Calculate the amount of speed increase based on how close the fish is to the bottom Y position
                float speedMultiplier = Mathf.Clamp01((topY - transform.position.y) / (topY - bottomY));
                float additionalSpeed = speedMultiplier * 2f;
                Debug.Log(additionalSpeed);
                currentSpeed += additionalSpeed;
            }
        }

        // Update the scale based on the level of the third item in the shop
        int scaleItemLevel = shopManager.shopItems[3, 3];
        currentScale = baseScale + (scaleItemLevel * scaleIncreasePerLevel);

        // Update the rotate speed based on the level of the third item in the shop
        int rotateSpeedItemLevel = shopManager.shopItems[3, 7];
        currentRotateSpeed = baseRotateSpeed + (rotateSpeedItemLevel * rotateSpeedIncreasePerLevel);
    }

    private void MoveAndRotateFish()
    {
        // Calculate the direction vector from the fish to the target position
        Vector3 direction = targetPosition - transform.position;

        // Calculate the angle in degrees from the fish's forward direction to the target direction
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Get the current rotation angle of the fish
        float currentAngle = transform.rotation.eulerAngles.z;

        // Smoothly interpolate the current angle towards the target angle based on the currentRotateSpeed
        float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, currentRotateSpeed * Time.deltaTime);

        // Apply the calculated angle to the fish's rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Move the fish forward in the direction it is facing
        Vector3 forwardDirection = transform.right;  // Assuming the fish is facing along its local right axis
        transform.position += forwardDirection * currentSpeed * Time.deltaTime;

        // Flip the fish sprite based on the direction of movement (avoid upside down)
        if (currentAngle < 90 || currentAngle > 270)
        {
            transform.localScale = new Vector3(-currentScale, currentScale, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-currentScale, -currentScale, 1f);
        }
    }
}