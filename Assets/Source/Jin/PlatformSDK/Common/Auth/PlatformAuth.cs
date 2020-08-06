using Jin.PlatformSDK.Unity;
using System;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Auth
{
    public class PlatformAuth
    {
        private static IPlatformAuth platformAuth;

        public delegate void ReceiveLoginResult(Error error, string token);
        public delegate void ReceiveResult(Error error, string data);

        static PlatformAuth()
        {
            if (platformAuth != null)
                return;

#if UNITY_ANDROID
            platformAuth = new PlatformAndroidAuth();
#elif UNITY_IOS
            // platformAuth = new PlatformIOSAuth();
#else
            // platformAuth = new PlatformEditorAuth();
#endif
        }

        public static string AuthInitialize(string providerName)
        {
            Debug.Log($"Andro_Platform Send {PlatformAuthData.AUTH_INITIALIZE}");

            return platformAuth.AuthInitialize(providerName);
        }

        public static void Login(string providerName, ReceiveLoginResult onLogin)
        {
            Debug.Log($"Andro_Platform Send {PlatformAuthData.AUTH_LOGIN}");

            if (platformAuth == null)
                return;

            platformAuth.Login(providerName, onLogin);
        }

        public static void Logout(ReceiveResult onLogout)
        {
            Debug.Log($"Andro_Platform Send {PlatformAuthData.AUTH_LOGOUT}");

            if (platformAuth == null)
                return;

            platformAuth.Logout(onLogout);
        }
        
        public static string IsLogin()
        {
            Debug.Log($"Andro_Platform Send {PlatformAuthData.AUTH_IS_LOGIN}");

            if (platformAuth == null)
                return string.Empty;

            return platformAuth.IsLogin();
        }
    }
}