using UnityEngine;

public class SceneTransition : MonoBehaviour {

    public void ChangeSceneByID(int sceneID) {
        SceneTransitionManager.Instance.LoadSceneByID(sceneID);
    }

    public void ChangeSceneByName(string sceneName) {
        SceneTransitionManager.Instance.LoadSceneByName(sceneName);
    }
}
