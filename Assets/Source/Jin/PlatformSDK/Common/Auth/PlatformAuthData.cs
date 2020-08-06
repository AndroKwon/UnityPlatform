#if UNITY_EDITOR || UNITY_ANDROID

namespace Jin.PlatformSDK.Common.Auth
{

    public class PlatformAuthData
    {
        public class SendInitialize
        {
            public string providerName;
            public string facebookApplicationId;
        }

        public class SendLogin
        {
            public string providerName;
        }


        public class RecvLogout
        {
            public string providerName;
        }

        public class RecvLogin
        {
            public string providerName;
            public string token;
        }

    }

}
#endif