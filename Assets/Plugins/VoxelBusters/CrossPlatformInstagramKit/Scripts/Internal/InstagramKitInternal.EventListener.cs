using VoxelBusters.InstagramKit.Common.DesignPatterns;

namespace VoxelBusters.InstagramKit
{
	using Internal;
	internal partial class InstagramKitInternal : SingletonPattern<InstagramKitInternal>, INativeCallbackListener
    {
		#region Events

		private InstagramKitDelegates.ShareCompletion						ShareFinishedEvent;

		#endregion
	

		#region INativeCallbackListener implementation

		public void OnShareFinished (bool success, string error)
		{
			if (ShareFinishedEvent != null) 
			{
				ShareFinishedEvent(success, error);
			}
		}

		#endregion
    }
}