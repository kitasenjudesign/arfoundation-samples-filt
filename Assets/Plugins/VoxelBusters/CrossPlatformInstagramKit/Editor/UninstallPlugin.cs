using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && !(UNITY_WEBPLAYER || UNITY_WEBGL || NETFX_CORE)
using UnityEditor;
using System.IO;
using VoxelBusters.InstagramKit.Common.Utility;

namespace VoxelBusters.InstagramKit.Internal
{
	public class UninstallPlugin
	{
		#region Constants
	
		private const	string	kUninstallAlertTitle	= "Uninstall - Cross Platform Instagram Kit";
		private const	string	kUninstallAlertMessage	= "Backup before doing this step to preserve changes done with in this plugin. This deletes files only related to InstagramKit plugin. Do you want to proceed?";

		private static string[] kPluginFolders	=	new string[]
		{
			Constants.kAndroidPluginsInstagramKitPath,
            Constants.kPluginAssetsPath
		};
		
		#endregion	
	
		#region Methods
	
		public static void Uninstall()
		{
			bool _startUninstall = EditorUtility.DisplayDialog(kUninstallAlertTitle, kUninstallAlertMessage, "Uninstall", "Cancel");

			if (_startUninstall)
			{
				foreach (string _eachFolder in kPluginFolders)
				{
					string _absolutePath = AssetDatabaseUtils.AssetPathToAbsolutePath(_eachFolder);

					if (Directory.Exists(_absolutePath))
					{
						Directory.Delete(_absolutePath, true);
						
						// Delete meta files.
						FileOperations.Delete(_absolutePath + ".meta");
					}
				}

				AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Cross Platform Instagram Kit",
				                            "Uninstall successful!", 
				                            "Ok");
			}
		}
		
		#endregion
	}
}
#endif