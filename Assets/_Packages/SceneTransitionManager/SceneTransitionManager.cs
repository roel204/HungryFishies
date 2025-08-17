using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour {
    public static SceneTransitionManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform transitionPanel;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;

    [Header("Animation Settings")]
    public float animationDuration = 1f;
    public bool animateFade = true;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool animateScale = false;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideInInspector] public bool allowAnimation = true;
    [HideInInspector] public bool isTransitioning = false;

    private float currentAnimationValue = 1f;
    private Vector3 defaultScale;
    private Coroutine textDotCoroutine;
    private string loadingMessage = "Loading";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        defaultScale = transitionPanel.localScale;

        StartCoroutine(AnimateOut(animationDuration));
    }

    public void LoadSceneByName(string sceneName) {
        StartCoroutine(SceneTransition(sceneName: sceneName));
    }

    public void LoadSceneByID(int sceneID) {
        StartCoroutine(SceneTransition(sceneID));
    }

    public void ReloadCurrentScene() {
        int currentSceneID = SceneManager.GetActiveScene().buildIndex;
        LoadSceneByID(currentSceneID);
    }

    private IEnumerator SceneTransition(int sceneID = -1, string sceneName = null) {
        if (isTransitioning) yield break;
        isTransitioning = true;

        SetLoadingMessage("Loading");
        yield return AnimateIn(animationDuration);

        // Start async loading scene
        AsyncOperation asyncLoad;
        if (sceneID != -1) {
            asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        } else {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        }
        asyncLoad.allowSceneActivation = false;

        // Wait until loading is done and update loading bar
        while (!asyncLoad.isDone) {
            SetLoadingBar(asyncLoad.progress);

            // When load reaches 90%, activate scene
            if (asyncLoad.progress >= 0.9f) {
                SetLoadingBar(1f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Wait incase any script needs to stop the animation
        yield return new WaitForSeconds(0.2f);
        yield return AnimateOut(animationDuration);
        isTransitioning = false;
    }

    public IEnumerator AnimateIn(float duration) {
        if (!allowAnimation) yield break;

        // If already animating in, wait for current animation to finish and exit
        if (currentAnimationValue > 0f) {
            while (currentAnimationValue < 1f) {
                yield return null;
            }
            yield break;
        }

        yield return StartCoroutine(AnimateTransition(1f, duration));

        // Show UI elements
        if (loadingText != null) {
            textDotCoroutine ??= StartCoroutine(TextDotAnimation());
            loadingText.enabled = true;
        }
        if (loadingBar != null) {
            loadingBar.value = 0f;
            loadingBar.gameObject.SetActive(true);
        }
    }

    public IEnumerator AnimateOut(float duration) {
        if (!allowAnimation) yield break;

        // If already animating out, wait for current animation to finish and exit
        if (currentAnimationValue < 1f) {
            while (currentAnimationValue > 0f) {
                yield return null;
            }
            yield break;
        }

        // Hide UI elements
        if (textDotCoroutine != null) {
            StopCoroutine(textDotCoroutine);
            textDotCoroutine = null;
        }
        if (loadingText != null) loadingText.enabled = false;
        if (loadingBar != null) loadingBar.gameObject.SetActive(false);

        yield return StartCoroutine(AnimateTransition(0f, duration));
    }

    private IEnumerator AnimateTransition(float targetValue, float duration) {
        transitionPanel.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;

        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = transitionPanel.localScale;
        float startTime = Time.time;

        while (Time.time < startTime + duration) {
            float t = Mathf.Clamp01((Time.time - startTime) / duration);
            currentAnimationValue = t;

            // Animate Fade
            if (animateFade) {
                float alphaValue = Mathf.Lerp(startAlpha, targetValue, fadeCurve.Evaluate(t));
                canvasGroup.alpha = alphaValue;
            }

            // Animate Scale
            if (animateScale) {
                float scaleValue = Mathf.Lerp(startScale.x, targetValue, scaleCurve.Evaluate(t));
                transitionPanel.localScale = defaultScale * scaleValue;
            }

            yield return null;
        }

        // Snap to target at the end
        if (animateFade) canvasGroup.alpha = targetValue;
        if (animateScale) transitionPanel.localScale = defaultScale * targetValue;
        currentAnimationValue = targetValue;

        // Unblock raycasts and disable if animated out
        if (targetValue == 0f) {
            transitionPanel.gameObject.SetActive(false);
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void SetLoadingBar(float value) {
        if (loadingBar == null) return;
        loadingBar.value = value;
    }

    public void SetLoadingMessage(string message) {
        if (loadingText == null) return;
        loadingMessage = message;
        loadingText.text = loadingMessage;
    }

    private IEnumerator TextDotAnimation() {
        while (loadingText != null) {
            loadingText.text = loadingMessage;
            yield return new WaitForSeconds(0.2f);
            loadingText.text = loadingMessage + ".";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = loadingMessage + "..";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = loadingMessage + "...";
            yield return new WaitForSeconds(0.2f);
        }
    }
}