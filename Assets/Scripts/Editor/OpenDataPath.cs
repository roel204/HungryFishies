using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public static class OpenDataPath {

    [MenuItem("Tools/Open Persistent Data Path")]
    public static void OpenPersistentDataPath() {
        string path = Application.persistentDataPath;

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        var osFamily = SystemInfo.operatingSystemFamily;

        if (osFamily == OperatingSystemFamily.Windows) {
            Process.Start("explorer.exe", path.Replace('/', '\\'));
        } else if (osFamily == OperatingSystemFamily.MacOSX) {
            Process.Start("open", path);
        } else if (osFamily == OperatingSystemFamily.Linux) {
            Process.Start("xdg-open", path);
        }
    }
}
