using System.Collections;
using UnityEngine;

public class CanvasGroupController : MonoBehaviour {

    [Header("Fade Settings")]
    public float fadeTime = 0.5f;
    public bool fadeOutOnStart = true;

    private CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        if (fadeOutOnStart) FadeOut();
    }

    public void FadeOut() {
        StartCoroutine(Fade(1f, 0f));
    }

    public void FadeIn() {
        StartCoroutine(Fade(0f, 1f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha) {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < fadeTime) {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
