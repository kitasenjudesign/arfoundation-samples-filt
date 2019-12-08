namespace VoxelBusters.InstagramKit.Internal
{
    public interface INativeCallbackListener
    {
        void OnShareFinished(bool success, string error);
    }
}
