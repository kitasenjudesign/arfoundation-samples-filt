using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
namespace VoxelBusters.InstagramKit.Internal
{
	public partial class InstagramKitAndroid : MonoBehaviour, INativeService
	{

		private INativeCallbackListener 		m_listener;

#region Constructors

		public InstagramKitAndroid()
		{
			AndroidJavaClass _class = new AndroidJavaClass(Native.Class.NAME);
			Plugin = _class.CallStatic<AndroidJavaObject>("getInstance");
		}

#endregion

#region INativeService implementation

		public void Initialise(INativeCallbackListener listener)
		{
			m_listener = listener;
		}

		public bool IsAvailable()
		{
			bool isAvailable = Plugin.Call<bool>(Native.Methods.IS_AVAILABLE);
			return isAvailable;
		}

        public void Share(FeedContent content)
        {
            bool isVideoSharing = (content.ContentType == eShareContentType.Video);
            Plugin.Call(Native.Methods.SHARE_AS_FEEd, content.ContentDataPath, isVideoSharing);
        }

        public void Share (StoryContent content)
		{
			bool isVideoSharing = (content.ContentType == eShareContentType.Video);
            Plugin.Call(Native.Methods.SHARE_AS_STORY, content.ContentDataPath, content.Sticker != null ? content.Sticker.FilePath : null, content.AttachmentUrl, isVideoSharing);
		}
		 
#endregion
	}
}
#endif