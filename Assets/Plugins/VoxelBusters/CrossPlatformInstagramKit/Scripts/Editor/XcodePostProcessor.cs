using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using System.IO;
using System;

namespace VoxelBusters.InstagramKit.Internal
{
    public class XcodePostProcessor : MonoBehaviour
    {
        [PostProcessBuildAttribute(0)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
        {


            // Stop processing if its other than iOS
            if (buildTarget != BuildTarget.iOS)
                return;

            // Info.plist modifications
            string          plistPath   = Path.Combine(buildPath, "Info.plist");
            PlistDocument   plist       = new PlistDocument();

            plist.ReadFromFile(plistPath);

            PlistElementDict rootDictionary = plist.root;


            rootDictionary.SetString("NSPhotoLibraryUsageDescription", "${PRODUCT_NAME} uses Photo Library");


            // Add application query schemes
            PlistElementArray applicationQuerySchemesArray = GetArray(rootDictionary, "LSApplicationQueriesSchemes");
            AddStringToArrayIfRequired(applicationQuerySchemesArray, "instagram");
            AddStringToArrayIfRequired(applicationQuerySchemesArray, "instagram-stories");


            // Save changes to disk
            plist.WriteToFile(plistPath);
        }

        // this add new array if the key doesn't exist. If exists, returns it.
        private static PlistElementArray GetArray(PlistElementDict source, string key)
        {

            PlistElementArray  array = null;
            if(source.values.ContainsKey(key))
                array = source.values[key] as PlistElementArray;

            if (array == null)
            {
                array = source.CreateArray(key);
            }

            return array;
        }

        private static void AddStringToArrayIfRequired(PlistElementArray array, string value)
        {
            foreach(PlistElement each in array.values)
            {
                if(each is PlistElementString)
                {
                    string existingValue = each.AsString();
                    if(!string.IsNullOrEmpty(existingValue) && existingValue.Equals(value))
                    {
                       return;
                    }
                }
            }

            // We reached here as value doesn't exist
            array.AddString(value);
        }
}
}

