
namespace Jin.PlatformSDK.Common.Auth
{

    public class PlatformAuthData
    {
        public static string AUTH_INITIALIZE = "andro://authInitialize";
        public static string AUTH_LOGIN = "andro://login";
        public static string AUTH_LOGOUT = "andro://logout";
        public static string AUTH_IS_LOGIN = "andro://isLogin";

        public static string FacebookAppID = "378176306479216";

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