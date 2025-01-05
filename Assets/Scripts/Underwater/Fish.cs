using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public bool screenPressed = false;
    private KeyCode mouseButton = KeyCode.Mouse0;
    private Vector3 mousePos;
    private Animator animator;

    private float currentVelocity = 0f;
    private readonly float acceleration = 4f;

    private float currentSpeed;
    public float baseSpeed;
    private float speedIncreasePerLevel = 0.8f;

    private float currentScale;
    private float baseScale = 1f;
    private float scaleIncreasePerLevel = 0.2f;

    private float currentRotateSpeed;
    private float baseRotateSpeed;
    private float rotateSpeedIncreasePerLevel = 40f;

    private UpgradeManager upgradeManager;
    private FollowCamera followCamera;

    private bool autoSwimEnabled;
    private bool lrTurn;

    private bool turnLeft = false;
    private bool turnRight = false;

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

        // Add increases for some fish
        if (selectedFishData.abilities.Contains("biggerUpgrades"))
        {
            speedIncreasePerLevel += 0.2f;
            scaleIncreasePerLevel += 0.1f;
            rotateSpeedIncreasePerLevel += 10f;
        }

        // Set the sprite based on the selected fish's name
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/Fishies/{selectedFishData.id}");

        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"Animations/Fishies/{selectedFishData.id}");

        // Set the BoxCollider2D size to match the sprite's size
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            // Match the collider size to the sprite's bounds
            boxCollider.size = spriteRenderer.bounds.size;
            boxCollider.offset = spriteRenderer.bounds.center - transform.position;
        }

        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        followCamera = FindFirstObjectByType<FollowCamera>();

        autoSwimEnabled = PlayerPrefs.GetInt("AutoSwim", 0) == 1;

        lrTurn = PlayerPrefs.GetInt("lrTurn", 0) == 1;

        if (lrTurn) { autoSwimEnabled = true; };

        MoveAndRotateFish();
    }

    private void Update()
    {
        MoveAndRotateFish();

        // Update the swimming speed based on the level of the first item in the shop
        int speedItemLevel = upgradeManager.GetLevel("FishSpeed");
        currentSpeed = baseSpeed + (speedItemLevel * speedIncreasePerLevel);

        // Speed Increase for Ray
        if (GameManager.instance.fishDataList[GameManager.instance.selectedFish].abilities.Contains("groundSpeed"))
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
        int scaleItemLevel = upgradeManager.GetLevel("FishSize");
        currentScale = baseScale + (scaleItemLevel * scaleIncreasePerLevel);

        // Update the rotate speed based on the level of the third item in the shop
        int rotateSpeedItemLevel = upgradeManager.GetLevel("FishRotateSpeed");
        currentRotateSpeed = baseRotateSpeed + (rotateSpeedItemLevel * rotateSpeedIncreasePerLevel);
    }

    private void MoveAndRotateFish()
    {
        Vector3 direction;
        float currentAngle = transform.rotation.eulerAngles.z;

        if (Input.GetKey(mouseButton))
        {
            screenPressed = true;
            Vector3 newMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newMousePosition.z = 0f;
            mousePos = newMousePosition;
        }
        else
        {
            screenPressed = false;
        }

        // Always move the fish forward when autoSwim is enabled or the player is following the mouse
        if (screenPressed || autoSwimEnabled)
        {
            if (lrTurn)
            {
                float targetAngle2 = currentAngle;
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || turnLeft)
                {
                    targetAngle2 += currentRotateSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || turnRight)
                {
                    targetAngle2 -= currentRotateSpeed * Time.deltaTime;
                }
                float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle2, currentRotateSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            }
            else if (screenPressed)
            {
                float targetAngle1;
                direction = mousePos - transform.position;
                targetAngle1 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Smoothly interpolate the current angle towards the target angle based on the currentRotateSpeed
                float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle1, currentRotateSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            }

            // Accelerate the fish forward in the direction it is facing
            Vector3 forwardDirection = transform.right;
            currentVelocity = Mathf.MoveTowards(currentVelocity, currentSpeed, acceleration * Time.deltaTime);
            transform.position += currentVelocity * Time.deltaTime * forwardDirection;
        }
        else
        {
            // Gradually decrease the velocity when not following the mouse and autoSwim is disabled
            currentVelocity = Mathf.MoveTowards(currentVelocity, 0f, acceleration * Time.deltaTime);

            // Continue moving the fish forward based on the reduced velocity
            if (currentVelocity > 0f)
            {
                transform.position += transform.right * currentVelocity * Time.deltaTime;
            }
        }

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, followCamera.minX, followCamera.maxX),
            Mathf.Clamp(transform.position.y, followCamera.minY, followCamera.maxY)
        );

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

    public void LeftPointerDown()
    {
        turnLeft = true;
        turnRight = false;
    }

    public void LeftPointerUp()
    {
        turnLeft = false;
    }

    public void RightPointerDown()
    {
        turnRight = true;
        turnLeft = false;
    }

    public void RightPointerUp()
    {
        turnRight = false;
    }
}