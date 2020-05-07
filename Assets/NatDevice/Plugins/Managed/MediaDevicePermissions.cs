/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using UnityEngine;
    using UnityEngine.Android;
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using Internal;

    public sealed partial class MediaDeviceQuery {
        
        #region --Client API--
        /// <summary>
        /// Reuest permissions to use media devices from the user.
        /// </summary>
        [Doc(@"RequestPermissions")]
        public static Task<bool> RequestPermissions<T> () where T : IMediaDevice {
            // Check type
            var camera = typeof(ICameraDevice).IsAssignableFrom(typeof(T));
            var microphone = typeof(IAudioDevice).IsAssignableFrom(typeof(T));
            if (!camera && !microphone)
                return Task.FromResult(true);
            // Request
            var permissionTask = new TaskCompletionSource<bool>();
            var requester = new GameObject("MediaDeviceQuery Permissions Helper").AddComponent<MediaDeviceQueryPermissionsHelper>();
            requester.Request(camera, granted => {
                permissionTask.SetResult(granted);
                MonoBehaviour.Destroy(requester);
            });
            return permissionTask.Task;
        }
        #endregion


        #region --Operations--

        private sealed class MediaDeviceQueryPermissionsHelper : MonoBehaviour {

            private void Awake () => DontDestroyOnLoad(this.gameObject);
 
            public void Request (bool camera, Action<bool> completionHandler) {
                switch (Application.platform) {
                    case RuntimePlatform.Android: StartCoroutine(RequestAndroid(camera, completionHandler)); break;
                    case RuntimePlatform.OSXEditor: goto case RuntimePlatform.IPhonePlayer;
                    case RuntimePlatform.OSXPlayer: goto case RuntimePlatform.IPhonePlayer;
                    case RuntimePlatform.WindowsEditor: goto case RuntimePlatform.IPhonePlayer;
                    case RuntimePlatform.WindowsPlayer: goto case RuntimePlatform.IPhonePlayer;
                    case RuntimePlatform.IPhonePlayer: StartCoroutine(RequestiOS(camera, completionHandler)); break;
                    default: completionHandler(true); break;
                }
            }

            private IEnumerator RequestAndroid (bool camera, Action<bool> completionHandler) {
                var permission = camera ? Permission.Camera : Permission.Microphone;
                if (Permission.HasUserAuthorizedPermission(permission))
                    completionHandler(true);
                else {
                    Permission.RequestUserPermission(permission);
                    yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(permission));
                    completionHandler(true);
                }
            }

            private IEnumerator RequestiOS (bool camera, Action<bool> completionHandler) {
                var permission = camera ? UserAuthorization.WebCam : UserAuthorization.Microphone;
                if (Application.HasUserAuthorization(permission))
                    completionHandler(true);
                else {
                    yield return Application.RequestUserAuthorization(permission);
                    completionHandler(Application.HasUserAuthorization(permission));
                }
            }
        }
        #endregion
    }
}