using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit.Internal
{
	public partial class InstagramKitDefaultPlatform : MonoBehaviour, INativeService
	{
		private const string kUnSupportedFeature = "Not supported on Editor Platform";

		public void ShareFinished(string data)
		{
			bool 	success = false;
			string	error	= null;


			m_listener.OnShareFinished (success, error);
		}


		private INativeCallbackListener 		m_listener;

        #region INativeService implementation

        public void Initialise(INativeCallbackListener listener)
        {
            m_listener = listener;
        }

        public bool IsAvailable()
        {
            return false;
        }

        public void Share (FeedContent content)
		{
			m_listener.OnShareFinished (false, kUnSupportedFeature);
		}

        public void Share(StoryContent content)
        {
            m_listener.OnShareFinished(false, kUnSupportedFeature);
        }

        #endregion
    }
}