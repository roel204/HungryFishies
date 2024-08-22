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

    private float currentRotateSpeed;
    private float baseRotateSpeed = 150f;
    private float rotateSpeedIncreasePerLevel = 40f;

    private Vector3 targetPosition = new(10, 10, 0);

    private ShopManager shopManager;

    private void Start()
    {
        currentSpeed = baseSpeed;
        currentScale = baseScale;
        currentRotateSpeed = baseRotateSpeed;

        shopManager = FindObjectOfType<ShopManager>();

        // Set the sprite based on the selected fish
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fishSkins[GameManager.instance.selectedFish];

        // Set the BoxCollider2D size to match the sprite's size
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            // Match the collider size to the sprite's bounds
            boxCollider.size = spriteRenderer.bounds.size;
            boxCollider.offset = spriteRenderer.bounds.center - transform.position;
        }

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