using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit
{
    public class StoryContent : ShareContent
    {
        #region Properties

        public Sticker Sticker
        {
            get;
            private set;
        }

        public string AttachmentUrl
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public StoryContent(string contentDataFilePath, bool isVideoFileType = false) : base(contentDataFilePath, isVideoFileType)
        {

        }

        #endregion


        #region Data Setters

        public void SetSticker(Sticker sticker)
        {
            Sticker = sticker;
        }

        public void SetAttachmentUrl(string attachmentUrl)
        {
            AttachmentUrl = attachmentUrl;
        }

        #endregion
    }
}
