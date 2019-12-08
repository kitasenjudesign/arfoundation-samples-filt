using VoxelBusters.InstagramKit.Common.DesignPatterns;

namespace VoxelBusters.InstagramKit
{
	using Internal;
	internal partial class InstagramKitInternal : SingletonPattern<InstagramKitInternal>, INativeCallbackListener
    {
        INativeService m_service;

        #region Query Methods

        public bool IsAvailable()
        {
			return m_service.IsAvailable();
        }

        #endregion

        #region Share

        /*
		public void Share(FeedContent content, InstagramKitDelegates.ShareCompletion callback)
        {
			ShareFinishedEvent = callback;
			m_service.Share(content);
        }
        */

        public void Share(StoryContent content, InstagramKitDelegates.ShareCompletion callback)
        {
            ShareFinishedEvent = callback;
            m_service.Share(content);
        }

        #endregion


        #region Overriden Methods

        protected override void Init()
        {
            base.Init();

            // Not interested in non singleton instance
            if (instance != this)
                return;

#if (UNITY_ANDROID && !UNITY_EDITOR)
			m_service = this.gameObject.AddComponent<InstagramKitAndroid>();
#elif (UNITY_IOS && !UNITY_EDITOR)
			m_service = this.gameObject.AddComponent<InstagramKitIOS>();
#else
            m_service = this.gameObject.AddComponent<InstagramKitDefaultPlatform>();
#endif

            m_service.Initialise(this);
        }

        #endregion
    }
}