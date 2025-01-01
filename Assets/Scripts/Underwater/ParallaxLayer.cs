using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public bool horizontalMovement;
    public float parallaxEffectX;

    public bool verticalMovement;
    public float parallaxEffectY;

    private Transform camTransform;
    private Vector2 startPos;
    private Vector2 size;

    void Start()
    {
        camTransform = Camera.main.transform;
        startPos = transform.position;
        size = new Vector2(
            GetComponent<SpriteRenderer>().bounds.size.x,
            GetComponent<SpriteRenderer>().bounds.size.y
        );
    }

    void FixedUpdate()
    {
        float distanceX = horizontalMovement ? camTransform.position.x * parallaxEffectX : 0;
        float distanceY = verticalMovement ? camTransform.position.y * parallaxEffectY : 0;

        float movementX = horizontalMovement ? camTransform.position.x * (1 - parallaxEffectX) : 0;
        float movementY = verticalMovement ? camTransform.position.y * (1 - parallaxEffectY) : 0;

        if (horizontalMovement)
        {
            if (movementX > startPos.x + size.x)
            {
                startPos.x += size.x;
            }
            else if (movementX < startPos.x - size.x)
            {
                startPos.x -= size.x;
            }
        }

        if (verticalMovement)
        {
            if (movementY > startPos.y + size.y)
            {
                startPos.y += size.y;
            }
            else if (movementY < startPos.y - size.y)
            {
                startPos.y -= size.y;
            }
        }

        transform.position = new Vector3(
            startPos.x + distanceX,
            startPos.y + distanceY,
            transform.position.z
        );
    }
}
