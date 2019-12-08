using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit
{
	public class InstagramKitDelegates
	{
		#region Delegates

		/// <summary>
		/// Delegate that will be called upon completion of Share method.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		public delegate void ShareCompletion(bool success, string error);

		#endregion
	}
}