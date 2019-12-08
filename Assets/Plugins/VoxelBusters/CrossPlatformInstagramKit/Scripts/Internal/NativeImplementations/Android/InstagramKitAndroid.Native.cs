using UnityEngine;

#if UNITY_ANDROID
namespace VoxelBusters.InstagramKit.Internal
{
	public partial class InstagramKitAndroid : MonoBehaviour, INativeService 
	{
		#region Platform Native Info

		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME			= "com.voxelbusters.instagramkit.InstagramKitHandler";
			}

			// For holding method names
			internal class Methods
			{
				internal const string IS_AVAILABLE			= "isAvailable";
                internal const string SHARE_AS_FEEd         = "shareAsFeed";
                internal const string SHARE_AS_STORY		= "shareAsStory";
			}
		}

		#endregion

		#region  Native Access Variables

		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}

		#endregion
	}
}
#endif