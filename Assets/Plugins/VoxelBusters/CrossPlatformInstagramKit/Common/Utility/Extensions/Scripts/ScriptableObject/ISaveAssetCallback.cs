using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.InstagramKit.Common.Utility
{
	public interface ISaveAssetCallback
	{
		#region Methods

		void OnBeforeSave();

		#endregion
	}
}
