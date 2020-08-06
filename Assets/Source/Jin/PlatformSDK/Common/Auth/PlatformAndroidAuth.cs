#if UNITY_EDITOR || UNITY_ANDROID
using Jin.PlatformSDK.Unity;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Auth
{
    public class PlatformAndroidAuth : IPlatformAuth
    {
        private PlatformAuth.ReceiveLoginResult _onLogin;
        private PlatformAuth.ReceiveResult _onLogout;

        public PlatformAndroidAuth()
        {
            AuthDelegateManager.AddDelegate(PlatformAuth.AUTH_LOGIN, OnMessageLogin);
            AuthDelegateManager.AddDelegate(PlatformAuth.AUTH_LOGOUT, OnMessageLogout);
        }

        public string AuthInitialize(string providerName)
        {
            var vo = new PlatformAuthData.SendInitialize();
            vo.providerName = providerName;

            if(providerName == "facebook")
                vo.facebookApplicationId = PlatformAuth.FacebookAppID;

            var message = new UnityMessage();

            message.scheme = PlatformAuth.AUTH_INITIALIZE;
            message.jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(vo);

            return AndroidMessageSender.Instance.GetSync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        public void Login(string providerName, PlatformAuth.ReceiveLoginResult onLogin)
        {
            _onLogin = onLogin;

            var vo = new PlatformAuthData.SendLogin();
            vo.providerName = providerName;

            var message = new UnityMessage();

            message.scheme = PlatformAuth.AUTH_LOGIN;
            message.jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(vo);

            AndroidMessageSender.Instance.GetAsync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        public void Logout(PlatformAuth.ReceiveResult onLogout)
        {
            _onLogout = onLogout;

            var message = new UnityMessage();

            message.scheme = PlatformAuth.AUTH_LOGOUT;

            AndroidMessageSender.Instance.GetAsync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        public string IsLogin()
        {
            var message = new UnityMessage();

            message.scheme = PlatformAuth.AUTH_IS_LOGIN;

            return AndroidMessageSender.Instance.GetSync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        private void OnMessageLogin(Message message)
        {
            Debug.Log($"Andro_Platform Recv {PlatformAuth.AUTH_LOGIN}");

            var error = JsonConvert.DeserializeObject<Error>(message.error);
            var login = JsonConvert.DeserializeObject<PlatformAuthData.RecvLogin>(message.jsonData);

            _onLogin?.Invoke(error, login.token);
        }

        private void OnMessageLogout(Message message)
        {
            Debug.Log($"Andro_Platform Recv {PlatformAuth.AUTH_LOGOUT}");

            var error = JsonConvert.DeserializeObject<Error>(message.error);
            var logout = JsonConvert.DeserializeObject<PlatformAuthData.RecvLogout>(message.jsonData);

            _onLogout?.Invoke(error, logout.providerName);
        }

    }
}
#endif