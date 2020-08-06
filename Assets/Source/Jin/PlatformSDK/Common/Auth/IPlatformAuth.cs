#if UNITY_EDITOR || UNITY_ANDROID

using System;

namespace Jin.PlatformSDK.Common.Auth
{
    public interface IPlatformAuth
    {
        string AuthInitialize(string providerName);

        void Login(string providerName, PlatformAuth.ReceiveLoginResult onLogin);

        void Logout(PlatformAuth.ReceiveResult onLogout);

        string IsLogin();
    }
}
#endif