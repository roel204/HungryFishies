using UnityEngine;
using UnityEngine.Events;

public class ToggleUI : MonoBehaviour {

    public UnityEvent onOpen;
    public UnityEvent onClose;

    public void Open() {
        onOpen?.Invoke();
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }
}
