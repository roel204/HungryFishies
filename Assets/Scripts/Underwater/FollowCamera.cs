using System.Linq;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    // Define the boundaries within which the camera can move
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (GameManager.instance.fishDataList[GameManager.instance.selectedFish].abilities.Contains("viewDistance"))
        {
            cam.orthographicSize = 7;
        }
        else
        {
            cam.orthographicSize = 5;
        }
    }

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
