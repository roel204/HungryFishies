using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    // Define the boundaries within which the camera can move
    private readonly float minX = -19.1f;
    private readonly float maxX = 19.1f;
    private readonly float minY = -10.7f;
    private readonly float maxY = 10.7f;

    private void LateUpdate()
    {
        if (target != null)
        {
            // Get the target position
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, -10);

            // Get camera zoom details
            float cameraHeight = Camera.main.orthographicSize;
            float cameraWidth = cameraHeight * Camera.main.aspect;

            // Clamp the position to the border + camera zoom
            targetPos.x = Mathf.Clamp(targetPos.x, minX + cameraWidth, maxX - cameraWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, minY + cameraHeight, maxY - cameraHeight);

            transform.position = targetPos;
        }
    }
}
