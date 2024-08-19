using System.Collections;
using TMPro;
using UnityEngine;

public class ShopMenuController : MonoBehaviour
{
    public TextMeshProUGUI shopButtonText;
    private bool isShopOpen = false;
    private RectTransform parentRectTransform;
    private Vector2 start = new Vector2(0f, 0f); // Desired start position
    private Vector2 end = new Vector2(400f, 0f); // Desired end position

    private void Start()
    {
        parentRectTransform = GetComponent<RectTransform>();
    }

    public void ToggleShop()
    {
        if (isShopOpen)
        {
            CloseShop();
        }
        else
        {
            StartCoroutine(OpenShop());
        }

        isShopOpen = !isShopOpen;
    }

    private IEnumerator OpenShop()
    {
        LeanTween.move(parentRectTransform, end, 0.2f).setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(0.2f);

        shopButtonText.text = "<";

        Time.timeScale = 0f; // Pause the game
    }

    private void CloseShop()
    {
        Time.timeScale = 1f; // Unpause the game

        LeanTween.move(parentRectTransform, start, 0.2f).setEase(LeanTweenType.easeOutQuad);

        shopButtonText.text = ">";
    }
}
