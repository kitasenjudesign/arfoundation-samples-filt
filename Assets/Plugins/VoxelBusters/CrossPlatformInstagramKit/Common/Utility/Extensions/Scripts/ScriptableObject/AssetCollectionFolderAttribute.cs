using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Attribute = System.Attribute;

namespace VoxelBusters.InstagramKit.Common.Utility
{
	public class AssetCollectionFolderAttribute : Attribute
	{
		#region Properties

		public string FolderName
		{
			get;
			private set;
		}

		#endregion

		#region Constructors

        public AssetCollectionFolderAttribute(string folderName)
		{
			// set property
			FolderName	= folderName;
		}

		#endregion
	}
}