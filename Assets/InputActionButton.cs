using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputActionButton : MonoBehaviour {

    public InputActionReference input;
    public bool submitOnPress = true;
    public bool submitOnRelease = false;

    private Button button;

    private void OnEnable() {
        button = GetComponent<Button>();
        if (button == null) {
            Debug.LogError("InputActionButton requires a Button component.");
            return;
        }

        if (input != null) {
            input.action.started += OnInputStarted;
            input.action.canceled += OnInputCanceled;
        }
    }

    private void OnDisable() {
        if (input != null) {
            input.action.started -= OnInputStarted;
            input.action.canceled -= OnInputCanceled;
        }
    }

    // Simulate pointer down
    private void OnInputStarted(InputAction.CallbackContext ctx) {
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        if (submitOnPress) ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }

    // Simulate pointer up
    private void OnInputCanceled(InputAction.CallbackContext ctx) {
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        if (submitOnRelease) ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }
}
