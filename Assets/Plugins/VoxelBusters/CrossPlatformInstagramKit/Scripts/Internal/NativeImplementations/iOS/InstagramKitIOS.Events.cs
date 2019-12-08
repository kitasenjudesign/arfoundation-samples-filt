using UnityEngine;
using System.Collections;
using VoxelBusters.InstagramKit.Common.Utility;

#if UNITY_IOS
namespace VoxelBusters.InstagramKit.Internal
{
	public partial class InstagramKitIOS : MonoBehaviour, INativeService
	{
		public void ShareFinished(string error)
		{
            bool isSuccessful = string.IsNullOrEmpty(error);

			m_listener.OnShareFinished (isSuccessful, isSuccessful ? "" : error);
		}
	}
}
#endif