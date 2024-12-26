using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlocker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // These methods are required to block the raycast
    public void OnPointerClick(PointerEventData eventData)
    {
        // Do nothing; this prevents the event from passing through.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: Add functionality when the pointer enters the image.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Add functionality when the pointer exits the image.
    }

    // Ensure this GameObject is considered a raycast target
    private void Awake()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.blocksRaycasts = true;
    }
}
