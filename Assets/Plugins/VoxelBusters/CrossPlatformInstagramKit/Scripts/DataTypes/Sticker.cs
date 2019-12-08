using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit
{
    public class Sticker
    {
        #region Properties

        public string FilePath
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public Sticker(string filePath)
        {
            FilePath = filePath;
        }

        #endregion
    }
}