using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using VoxelBusters.InstagramKit.Common.Utility;

namespace VoxelBusters.InstagramKit.Internal
{
    public class Menu
    {
        #region Constants

        // Menu item names
        const string kMenuNodeMainNode = "Window/Voxel Busters/Cross Platform Instagram Kit";
        const string kSupportURL = "https://join.skype.com/ln8JcMryHXpv";
        const string kTutorialURL = "https://assetstore.instagramkit.voxelbusters.com";
        const string kSubscribePageURL = "http://bit.ly/2ESQfAg";

        #endregion

        # region Menu Items

        [MenuItem(kMenuNodeMainNode + "/Tutorials", false)]
        private static void OpenTutorialsLink()
        {
            Application.OpenURL(kTutorialURL);
        }

        [MenuItem(kMenuNodeMainNode + "/Support", false)]
        private static void OpenSupportLink()
        {
            Application.OpenURL(kSupportURL);
        }

        [MenuItem(kMenuNodeMainNode + "/Subscribe", false)]
        private static void OpenSubscribeLink()
        {
            Application.OpenURL(kSubscribePageURL);
        }

        [MenuItem(kMenuNodeMainNode + "/Uninstall", false)]
        private static void Uninstall()
        {
            UninstallPlugin.Uninstall();
        }

#if UNITY_ANDROID
        [MenuItem(kMenuNodeMainNode + "/Force Update Android Manifest", false)]
        private static void RegenerateAndroidManifest()
        {
            InstagramKitAndroidManifestGenerator.WriteAndroidManifestFile();
        }
#endif

        #endregion
    }
}
#endif