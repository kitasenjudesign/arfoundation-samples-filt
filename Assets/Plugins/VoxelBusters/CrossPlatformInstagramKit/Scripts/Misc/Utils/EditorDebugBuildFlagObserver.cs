using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

/*
#if UNITY_2018_1_OR_NEWER  // TODO : Implement this with this callback interface once we target above UNITY_2017
class InstagramKitPreProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport {
#else
class InstagramKitPreProcessor : IPreprocessBuild, IPostprocessBuild {
#endif
*/

namespace VoxelBusters.InstagramKit.Internal
{
	[InitializeOnLoad]
	public class EditorDebugBuildFlagObserver
	{

		#region Fields

		private static bool PreviousDebugFlagStatus = false;

		#endregion

		#region Delegates

		public delegate void OnDebugBuildFlagChanged();

		#endregion

		#region Events

		public static event OnDebugBuildFlagChanged debugBuildFlagChanged;

		#endregion



		static EditorDebugBuildFlagObserver()
		{
			EditorApplication.update -= MonitorDebugFlag;
			EditorApplication.update += MonitorDebugFlag;
		}

		private static void MonitorDebugFlag()
		{
			if(PreviousDebugFlagStatus != EditorUserBuildSettings.development)
			{
				PreviousDebugFlagStatus = EditorUserBuildSettings.development;
				NotifyChangedEvent();
			}
		}

		private static void NotifyChangedEvent()
		{
			if(debugBuildFlagChanged != null)
			{
				debugBuildFlagChanged();
			}
		}
	}
}
#endif
