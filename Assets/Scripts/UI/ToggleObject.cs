using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ToggleObject : MonoBehaviour {

    [Header("Options")]
    public bool stopTimeOnEnable = false;

    [Header("Input Actions")]
    public InputActionReference enableAction;
    public InputActionReference disableAction;
    public InputActionReference toggleAction;

    [Header("Events")]
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public UnityEvent onToggle;

    private void Awake() {
        if (enableAction != null) enableAction.action.performed += EnableAction;
        if (disableAction != null) disableAction.action.performed += DisableAction;
        if (toggleAction != null) toggleAction.action.performed += ToggleAction;
    }

    public void Enable() {
        onEnable?.Invoke();
        gameObject.SetActive(true);

        if (!stopTimeOnEnable) return;
        if (SceneTransitionManager.Instance == null || SceneTransitionManager.Instance.isTransitioning) return;
        Time.timeScale = 0f;
    }

    public void Disable() {
        gameObject.SetActive(false);
        onDisable?.Invoke();

        if (stopTimeOnEnable) Time.timeScale = 1f;
    }

    public void Toggle() {
        if (gameObject.activeSelf) {
            Disable();
        } else {
            Enable();
        }
        onToggle?.Invoke();
    }

    private void OnDestroy() {
        if (enableAction != null) enableAction.action.performed -= EnableAction;
        if (disableAction != null) disableAction.action.performed -= DisableAction;
        if (toggleAction != null) toggleAction.action.performed -= ToggleAction;
    }

    private void EnableAction(InputAction.CallbackContext ctx) {
        Enable();
    }
    private void DisableAction(InputAction.CallbackContext ctx) {
        Disable();
    }
    private void ToggleAction(InputAction.CallbackContext ctx) {
        Toggle();
    }
}
