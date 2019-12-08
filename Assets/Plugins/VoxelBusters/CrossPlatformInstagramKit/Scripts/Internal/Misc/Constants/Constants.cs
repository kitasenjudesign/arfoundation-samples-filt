using UnityEngine;
using System.Collections;

namespace VoxelBusters.InstagramKit.Internal
{
	public class Constants : MonoBehaviour
	{
		#region Errors

		public const string kDebugTag							= "Instagram Kit";
		public const string kNotSupportedInEditor				= "The operation could not be completed because the requested feature is not simulated in Unity Editor. Use your mobile device for testing this functionality.";

        #endregion

        #region Assets Path	

        public const string kPluginFolderName                   = "CrossPlatformInstagramKit";
		public const string	kRootAssetsPath						= "Assets";
		public const string	kVBCodebasePath						= "Assets/Plugins/VoxelBusters";

        public const string kPluginAssetsPath                   = kVBCodebasePath + "/" + kPluginFolderName;
		public const string kEditorAssetsPath					= kPluginAssetsPath + "/EditorResources";
		public const string kLogoPath							= kEditorAssetsPath + "/Logo/InstagramKit.png";

		#endregion
		
		#region GUI Style

		public const string kSampleUISkin						= "AssetStoreProductUISkin";//Available in AssetStoreProduct submodule
		public const string kSubTitleStyle  					= "sub-title";
		public const string	kButtonLeftStyle					= "ButtonLeft";
		public const string	kButtonMidStyle						= "ButtonMid";
		public const string	kButtonRightStyle					= "ButtonRight";

		#endregion

		#region Plugin Paths
        public const string kPluginsAndroidLibraryRootPath      = kRootAssetsPath +"/Plugins/Android";
        public const string kAndroidPluginsInstagramKitPath		= kPluginsAndroidLibraryRootPath + "/instagram_kit_lib";

		#endregion

		#region Asset Store Constants

		public const string	kFullVersionProductURL				= "http://u3d.as/1pMn";
        public const string kProductURL                         = kFullVersionProductURL;

		#endregion
	}
}