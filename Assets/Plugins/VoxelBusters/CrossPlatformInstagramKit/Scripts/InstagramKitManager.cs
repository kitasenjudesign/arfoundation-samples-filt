using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit
{
	using Internal;
	public class InstagramKitManager
	{
		#region Query Methods

		public static bool IsAvailable()
		{
			return InstagramKitInternal.Instance.IsAvailable();
		}

		
		#endregion

		#region Share

		/*public static void Share(FeedContent content, InstagramKitDelegates.ShareCompletion callback)
		{
			InstagramKitInternal.Instance.Share(content, callback);
		}*/

        public static void Share(StoryContent content, InstagramKitDelegates.ShareCompletion callback)
        {
            InstagramKitInternal.Instance.Share(content, callback);
        }

        #endregion
    }
}