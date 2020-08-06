#if UNITY_EDITOR || UNITY_ANDROID
using Jin.PlatformSDK.Unity;
using System;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Auth
{
    public class PlatformAuth
    {
        public static string AUTH_INITIALIZE = "andro://authInitialize";
        public static string AUTH_LOGIN = "andro://login";
        public static string AUTH_LOGOUT = "andro://logout";
        public static string AUTH_IS_LOGIN = "andro://isLogin";

        private static IPlatformAuth platformAuth;

        public delegate void ReceiveLoginResult(Error error, string token);
        public delegate void ReceiveResult(Error error, string data);

        public static string AuthInitialize(string providerName)
        {
            Debug.Log($"Andro_Platform Send {PlatformAuth.AUTH_INITIALIZE}");

            return platformAuth.AuthInitialize(providerName);
        }

        public static void Login(string providerName, ReceiveLoginResult onLogin)
        {
            Debug.Log($"Andro_Platform Send {PlatformAuth.AUTH_LOGIN}");

            if (platformAuth == null)
                return;

            platformAuth.Login(providerName, onLogin);
        }

        public static void Logout(ReceiveResult onLogout)
        {
            Debug.Log($"Andro_Platform Send {PlatformAuth.AUTH_LOGOUT}");

            if (platformAuth == null)
                return;

            platformAuth.Logout(onLogout);
        }
        
        public static string IsLogin()
        {
            Debug.Log($"Andro_Platform Send {PlatformAuth.AUTH_IS_LOGIN}");

            if (platformAuth == null)
                return string.Empty;

            return platformAuth.IsLogin();
        }

        public static void Initialize()
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
    }
}
#endif