using UnityEngine;
using System.Collections;
using VoxelBusters.InstagramKit.Common.Utility;

namespace VoxelBusters.InstagramKit.Internal

{
	public class NotifySettingsOnValueChangeAttribute : ExecuteOnValueChangeAttribute 
	{
		#region Constructors

        public  NotifySettingsOnValueChangeAttribute () : base ("OnPropertyModified")
		{}

		#endregion
	}
}