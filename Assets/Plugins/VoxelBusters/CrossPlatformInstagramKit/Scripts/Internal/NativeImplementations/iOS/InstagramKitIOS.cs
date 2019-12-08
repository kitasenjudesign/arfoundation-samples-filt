using UnityEngine;
using System.Collections;

#if UNITY_IOS
using System.Runtime.InteropServices;
using VoxelBusters.InstagramKit.Common.Utility;

namespace VoxelBusters.InstagramKit.Internal
{	
    public partial class InstagramKitIOS : MonoBehaviour, INativeService
	{
        private INativeCallbackListener m_listener;

        #region Native Methods

		[DllImport("__Internal")]
		private static extern bool instagramkit_isAvailable ();

        [DllImport("__Internal")]
        private static extern void instagramkit_share_feed(string contentDataPath, bool isVideoSharing);


        [DllImport("__Internal")]
        private static extern void instagramkit_share_story(string contentDataPath, string stickerPath, string attachmentURL, bool isVideoSharing);


        public void Initialise(INativeCallbackListener listener)
        {
            m_listener = listener;
        }

        public bool IsAvailable()
        {
            return instagramkit_isAvailable();
        }

        public void Share(FeedContent content)
        {
            bool isVideoSharing = (content.ContentType == eShareContentType.Video);
            instagramkit_share_feed(content.ContentDataPath, isVideoSharing);
        }

        public void Share(StoryContent content)
        {
            bool isVideoSharing = (content.ContentType == eShareContentType.Video);
            instagramkit_share_story(content.ContentDataPath, content.Sticker != null ? content.Sticker.FilePath : null, content.AttachmentUrl, isVideoSharing);
        }

        #endregion
    }
}
#endif