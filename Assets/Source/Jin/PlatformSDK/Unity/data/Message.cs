
namespace Jin.PlatformSDK.Unity
{
    public class Message
    {
        public string scheme;
        public string jsonData;
        public string extraData;
        public string error;
    }
    
    public class Error
    {
        public string code;
        public string message;
    }


    public class UnityMessage : Message
    {
        public string gameObjectName = string.Empty;
        public string responseMethodName = string.Empty;

        public UnityMessage()
        {
            responseMethodName = "OnAsyncEvent";
            gameObjectName = "Singleton_MessageReceiver";
        }
    }
}