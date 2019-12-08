using UnityEngine;
using System.Collections;
using VoxelBusters.InstagramKit.Common.Utility;

#if UNITY_ANDROID
namespace VoxelBusters.InstagramKit.Internal
{
	public partial class InstagramKitAndroid : MonoBehaviour, INativeService
	{
		public void ShareFinished(string status)
		{
			bool isSuccessful = bool.Parse(status);

			m_listener.OnShareFinished (isSuccessful, isSuccessful ? "" : "Failed to share content");
		}
	}
}
#endif