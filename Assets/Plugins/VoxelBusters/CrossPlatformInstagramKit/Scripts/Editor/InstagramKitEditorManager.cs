#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

using PlayerSettings	= VoxelBusters.InstagramKit.Common.Utility.PlayerSettings;
using VoxelBusters.InstagramKit.Common.Utility;

namespace VoxelBusters.InstagramKit.Internal
{
	[InitializeOnLoad]
	public class InstagramKitEditorManager : MonoBehaviour 
	{
		#region Constants

		private		const	string		kPrefsKeyBuildIdentifierForPlatform	= "instagram-kit-build-identifier-for-platform";

        private const	float 		kWaitingPeriod 	= 2f;

		#endregion

		#region Static Fields

		private		static 		float 		startTime;

        #endregion

        #region Static Constructor

#if !INSTAGRAM_KIT_HIBERNATE
        static InstagramKitEditorManager()
        {
            Reset();

            // regiser to editor update callback
            EditorApplication.update += EditorUpdate;

#if UNITY_ANDROID
            InstagramKitAndroidManifestGenerator.WriteAndroidManifestFile();
#endif
        }
		#endif

		#endregion

		#region Static Methods

		private static void Reset()
		{
			EditorApplication.update   -= EditorUpdate;

			// set default properties
			startTime 					= (float)EditorApplication.timeSinceStartup;

        }

		private static void EditorUpdate()
		{
			if (GetTimeSinceStart() < kWaitingPeriod)
				return;

			MonitorPlayerSettings();
		}

		private static float GetTimeSinceStart()
		{
			return (float)(EditorApplication.timeSinceStartup - startTime);
		}

		private static void MonitorPlayerSettings()
		{
            string _currentPlatform     = Application.platform.ToString();

			// check whether there's change in value
			string	_oldBuildIdentifier	= EditorPrefs.GetString(kPrefsKeyBuildIdentifierForPlatform, null);
            string	_curBuildIdentifier	= PlayerSettings.GetBundleIdentifier() + _currentPlatform;
			if (string.Equals(_oldBuildIdentifier, _curBuildIdentifier))
				return;

			// save copy of new value

            EditorPrefs.SetString(kPrefsKeyBuildIdentifierForPlatform, _curBuildIdentifier + _currentPlatform);
            InstagramKitAndroidManifestGenerator.WriteAndroidManifestFile();
        }

        #endregion
    }
}
#endif