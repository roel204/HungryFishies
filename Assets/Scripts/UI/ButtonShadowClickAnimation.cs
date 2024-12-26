using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Shadow))]
public class ButtonShadowClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Shadow shadow;
    private Vector3 originalPosition;
    private Vector2 originalShadowEffectDistance;

    private void Start()
    {
        // Get the RectTransform and Shadow components
        rectTransform = GetComponent<RectTransform>();
        shadow = GetComponent<Shadow>();

        if (rectTransform == null)
        {
            Debug.LogError("No RectTransform component found on this GameObject.");
            return;
        }

        if (shadow == null)
        {
            Debug.LogError("No Shadow component found on this GameObject.");
            return;
        }

        // Store the original position and shadow effect distance
        originalPosition = rectTransform.localPosition;
        originalShadowEffectDistance = shadow.effectDistance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Move the button down by half the shadow offset
        Vector3 moveOffset = new(originalShadowEffectDistance.x * 0.5f, originalShadowEffectDistance.y * 0.5f, 0);
        rectTransform.localPosition = originalPosition + moveOffset;

        // Adjust the shadow effect distance to make the shadow appear stationary
        shadow.effectDistance = originalShadowEffectDistance * 0.5f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset the button to its original position
        rectTransform.localPosition = originalPosition;

        // Restore the original shadow effect distance
        shadow.effectDistance = originalShadowEffectDistance;
    }
}
