using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneViewSnapshot {

    [MenuItem("Tools/Capture Scene View")]
    static void CaptureSceneView() {

        // Get the active scene view
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null) {
            Debug.LogError("No active scene view found.");
            return;
        }

        int width = sceneView.camera.pixelWidth;
        int height = sceneView.camera.pixelHeight;
        Texture2D capture = new(width, height);

        // Create a temporary render texture to capture the scene view
        sceneView.camera.Render();
        RenderTexture.active = sceneView.camera.targetTexture;

        // Read the pixels from the active render texture
        capture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        capture.Apply();
        byte[] bytes = capture.EncodeToPNG();

        // Get the path to save the screenshot
        string path = EditorUtility.SaveFilePanel("Save Scene View Screenshot", "", "SceneViewScreenshot.png", "png");
        if (string.IsNullOrEmpty(path)) return;

        // Save the screenshot to the specified path
        File.WriteAllBytes(path, bytes);

        Debug.Log($"Scene view screenshot saved to: {path}");
    }

    [MenuItem("Tools/Capture Game View")]
    static void CaptureGameView() {
        string path = EditorUtility.SaveFilePanel("Save Game View Screenshot", "", "GameViewScreenshot.png", "png");
        if (string.IsNullOrEmpty(path)) return;

        // Capture the screenshot and save it to the specified path
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log($"Game view screenshot saved to: {path}");
    }

}
