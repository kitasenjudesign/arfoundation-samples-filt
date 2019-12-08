using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit
{
    public abstract class ShareContent
    {
        #region Properties

        public eShareContentType ContentType
        {
            get;
            private set;
        }

        public string ContentDataPath
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public ShareContent(string contentDataFilePath, bool isVideoFileType)
        {
            ContentType = isVideoFileType ? eShareContentType.Video : eShareContentType.Photo;
            SetContentData(contentDataFilePath);
        }

        #endregion

        #region Data Setters

        private void SetContentData(string filePath)
        {
			if (filePath != null) 
			{
				if (filePath.StartsWith ("file://")) 
				{
					filePath.Replace("file://", "");
				}
			}

            ContentDataPath = filePath;
        }

        #endregion
    }
}
