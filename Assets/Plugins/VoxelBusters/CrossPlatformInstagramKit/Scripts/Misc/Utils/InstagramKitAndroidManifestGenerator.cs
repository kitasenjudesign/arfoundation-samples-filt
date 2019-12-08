using UnityEngine;
using System.Collections;
using VoxelBusters.InstagramKit.Common.Utility;
using System;

#if UNITY_EDITOR
using System.Xml;

namespace VoxelBusters.InstagramKit
{
	using Internal;
    public class InstagramKitAndroidManifestGenerator : AndroidManifestGenerator
    {
        #region Application Methods

        protected override void WriteApplicationProperties(XmlWriter xmlWriter)
        {
            WriteProviderInfo(xmlWriter);
        }


        private void WriteProviderInfo(XmlWriter _xmlWriter)
        {
            // Provider
            _xmlWriter.WriteComment("Custom File Provider. Sharing from internal folders");
            _xmlWriter.WriteStartElement("provider");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, "com.voxelbusters.instagramkit.extensions.FileProviderExtended");
                WriteAttributeString(_xmlWriter, "android", "authorities", null, string.Format("{0}.instagramkit.fileprovider", PlayerSettings.GetBundleIdentifier()));

                WriteAttributeString(_xmlWriter, "android", "exported", null, "false");
                WriteAttributeString(_xmlWriter, "android", "grantUriPermissions", null, "true");

                _xmlWriter.WriteStartElement("meta-data");
                {
                    WriteAttributeString(_xmlWriter, "android", "name", null, "android.support.FILE_PROVIDER_PATHS");
                    WriteAttributeString(_xmlWriter, "android", "resource", null, "@xml/file_paths");
                }
                _xmlWriter.WriteEndElement();
            }
            _xmlWriter.WriteEndElement();
        }

        #endregion

        #region Permission Methods

        protected override void WritePermissions(XmlWriter _xmlWriter)
        {

            //Internet access - Add by default as many features need this.
            WriteUsesPermission(_xmlWriter: _xmlWriter,
                                _name: "android.permission.INTERNET",
                                _comment: "Required for internet access");
        }

        #endregion

        #region Utility Methods

        public static void WriteAndroidManifestFile()
        {
            string _manifestFolderPath = Constants.kAndroidPluginsInstagramKitPath;

            if (AssetDatabaseUtils.FolderExists(_manifestFolderPath))
            {
                InstagramKitAndroidManifestGenerator _generator = new InstagramKitAndroidManifestGenerator();
#if UNITY_2017_1_OR_NEWER
                _generator.SaveManifest("com.voxelbusters.instagramkitplugin", _manifestFolderPath + "/AndroidManifest.xml", "11", "26");
#else
                _generator.SaveManifest("com.voxelbusters.instagramkitplugin", _manifestFolderPath + "/AndroidManifest.xml", "11", "24");
#endif
            }
        }

        #endregion
    }
}
#endif
