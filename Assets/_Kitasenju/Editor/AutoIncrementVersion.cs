using UnityEditor;
using UnityEditor.Callbacks;

public class AutoIncrementVersion{

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {

        if (target == BuildTarget.iOS) {
            //0 -> 1
            var buildNumber = int.Parse (PlayerSettings.iOS.buildNumber) + 1;
            PlayerSettings.iOS.buildNumber = "" + buildNumber;
        }
        else if (target == BuildTarget.Android) {
            //0 -> 1
            var versionCode = PlayerSettings.Android.bundleVersionCode + 1;
            PlayerSettings.Android.bundleVersionCode = versionCode;
        }
    }
}