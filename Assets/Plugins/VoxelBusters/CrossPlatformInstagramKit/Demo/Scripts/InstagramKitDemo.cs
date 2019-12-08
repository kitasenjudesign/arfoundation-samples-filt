using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.InstagramKit.Common.Utility;

namespace VoxelBusters.InstagramKit.Demos
{
	public class InstagramKitDemo : MonoBehaviour 
	{
        #region Fields

        [SerializeField]
        private Texture2D m_photo = null;

        [SerializeField]
		private Text m_statusText = null;

		[SerializeField]
		private Toggle m_addSticker = null;

		[SerializeField]
		private Toggle m_addAttachmentURL = null;

        [SerializeField]
		private string m_videoPath = null; // This will be downloaded from web and saved to local disk for sharing. You can directly pass the path of your recording.

		[SerializeField]
		private string m_stickerPath = null; // This will be downloaded from web and saved to local disk for sharing. You can directly pass the path of your recording.

		private bool m_isVideoDownloaded = false;

		#endregion

		#region Constants

		private const string SharingPictureName	= "photo.png";
		private const string SharingVideoName	= "video.mp4";
		private const string SharingStickerName	= "sticker.png";

		#endregion

		#region Unity LifeCycle Methods

		private void Awake()
		{
			DownloadPictureToPersistentPath();
			DownloadVideoToPersistentPath();
			DownloadStickerToPersistentPath();
		}

		#endregion

		#region Query Methods

		public void IsAvailable()
		{
			bool 	isAvailable	=	InstagramKitManager.IsAvailable();
			string 	message		= 	isAvailable ? "Instagram Kit is available!" : "Instagram Kit is not available. Instagram app may not be installed.";

			Log(message);
		}

		#endregion

		#region Share Methods


        public void ShareImage()
        {
            string path = Application.persistentDataPath + "/" + SharingPictureName;

            ShareAsStory(path, false);
        }

        public void ShareVideo()
        {
            if (m_isVideoDownloaded)
            {
                string path = Application.persistentDataPath + "/" + SharingVideoName;

                ShareAsStory(path, true);
            }
            else
            {
                Debug.LogError("Video is yet to download!");
            }
        }

        private void ShareAsStory(string path, bool isVideo)
        {
            StoryContent content = new StoryContent(path, isVideo);

            // Add any extra data like sticker or caption text or target attachment url
            Sticker sticker         = GetSticker();
            string attachmentURL    = GetAttachmentURL();

            content.SetSticker(sticker);
            content.SetAttachmentUrl(attachmentURL);

            InstagramKitManager.Share(content, OnShareComplete);

            /* // Another way to pass the callback
            InstagramKitManager.Instance.Share(content, (bool success, string error) => 
                {
                    string message  =  success ? "Successfully Shared" : "Failed to share " + error;
                    Log(message);       
                });
             */
        }

		#endregion

		#region Utility Methods

		private void Log(string message)
		{
			m_statusText.text	=	message;
			Debug.Log("[InstagramKit]" + message);
		}

		private Sticker GetSticker()
		{
			Sticker sticker = null;

			if (m_addSticker.isOn) 
			{
				sticker = new Sticker(Application.persistentDataPath + "/" + SharingStickerName);
			}

			return sticker;
		}

		private string GetAttachmentURL()
		{
			string attachmentURL = null;

			if (m_addAttachmentURL.isOn) 
			{
#if UNITY_ANDROID
				attachmentURL = "https://play.google.com/store/apps/details?id=com.yodo1.crossyroad&hl=en_IN";
#elif UNITY_IOS
                attachmentURL = "https://itunes.apple.com/us/app/crossy-road/id924373886?mt=8";
#else
                attachmentURL = "https://www.unity3d.com";
#endif
            }

			return attachmentURL;
		}

		private void DownloadPictureToPersistentPath()
		{
			byte[] data = null;

			if (SharingPictureName.EndsWith ("png")) 
			{
				data = ImageConversion.EncodeToPNG(m_photo);
			} 
			else 
			{
				data = ImageConversion.EncodeToJPG(m_photo);
			}

			FileOperations.WriteAllBytes(Application.persistentDataPath + "/" + SharingPictureName, data);
		}

		private void DownloadVideoToPersistentPath()
		{
			// Downloading if the file doesn't exist
			if (!FileOperations.Exists (Application.persistentDataPath + "/" + SharingVideoName)) {
				// Download video from given path
				DownloadAsset _newDownload	= new DownloadAsset (new URL (m_videoPath), true);
#pragma warning disable
                _newDownload.OnCompletion = (WWW _www, string _error) => {

					Debug.Log (string.Format ("[DownloadAsset] Asset download completed. Error= {0}.", _error.GetPrintableString ()));

					FileOperations.WriteAllBytes (Application.persistentDataPath + "/" + SharingVideoName, _www.bytes);
					m_isVideoDownloaded = true;
				};

				// Start download
				_newDownload.StartRequest ();
			} 
			else 
			{
                Debug.Log("Video file at path : " + Application.persistentDataPath);
				m_isVideoDownloaded = true;
			}
#pragma warning restore
        }

        private void DownloadStickerToPersistentPath()
		{
			// Download video from given path
			DownloadAsset _newDownload	= new DownloadAsset (new URL (m_stickerPath), true);
#pragma warning disable
            _newDownload.OnCompletion = (WWW _www, string _error) => {

				Debug.Log (string.Format ("[DownloadAsset] Asset download completed. Error= {0}.", _error.GetPrintableString ()) + Application.persistentDataPath);

				FileOperations.WriteAllBytes (Application.persistentDataPath + "/" + SharingStickerName, _www.bytes);
			};
#pragma warning restore

            // Start download
            _newDownload.StartRequest ();
		}

		#endregion

		#region Callback Methods

		private void OnShareComplete(bool success, string error)
		{
			string message	=  success ? "Successfully Shared" : "Failed to share " + error;
			Log(message);	
		}

        #endregion
    }
}