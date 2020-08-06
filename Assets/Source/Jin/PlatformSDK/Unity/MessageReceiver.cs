#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
using Jin.Utility;
using Newtonsoft.Json;
using UnityEngine;

namespace Jin.PlatformSDK.Unity
{
    public class MessageReceiver : Singleton<MessageReceiver>
    {
        public void OnAsyncEvent(string jsonString)
        {
            Debug.Log(string.Format("jsonString : {0}", jsonString), this);

            Message message = JsonConvert.DeserializeObject<Message>(jsonString);

            if(message == null)
            {
                Debug.Log("JSON parsing error. message object is null.", this);
                return;
            }

            AuthDelegateManager.MessageDelegate(message.scheme, message);
        }
    }
}
#endif