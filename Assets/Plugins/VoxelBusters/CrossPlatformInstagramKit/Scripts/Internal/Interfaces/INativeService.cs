using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit.Internal
{
    public interface INativeService
    {
		void Initialise(INativeCallbackListener listener);
        bool IsAvailable();

        void Share(FeedContent content);
        void Share(StoryContent content);
    }
}