using UnityEngine;

public class ShowSteeringButtons : MonoBehaviour {

    private void Start() {
        if (PlayerPrefs.GetInt("lrTurn", 0) == 1) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
