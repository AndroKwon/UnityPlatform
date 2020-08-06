#if UNITY_EDITOR || UNITY_ANDROID
using UnityEngine;

namespace Jin.PlatformSDK.Unity
{
    public sealed class AndroidMessageSender : IMessageSender
    {
        private static readonly AndroidMessageSender instance = new AndroidMessageSender();
        private const string ANDROID_PLUGIN_CLASS = "com.andro.unityplatformplugin.unitymessage.UnityMessageReceiver";
        private AndroidJavaClass jc = null;

        public static AndroidMessageSender Instance
        {
            get { return instance; }
        }

        private AndroidMessageSender()
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass(ANDROID_PLUGIN_CLASS);
            }

            Debug.Log("Initialize Message Receiver");

            MessageReceiver.Instance.Init();
        }

        public string GetSync(string jsonString)
        {
            Debug.Log(string.Format("jsonString : {0}", jsonString));
            
            string retValue = jc.CallStatic<string>("getSync", jsonString);

            if (string.IsNullOrEmpty(retValue) == false)
            {
                Debug.Log(string.Format("retValue : {0}", retValue));                
            }

            return retValue;
        }

        public void GetAsync(string jsonString)
        {
            Debug.Log(string.Format("jsonString : {0}", jsonString));

            jc.CallStatic("getAsync", jsonString);
        }
    }
}
#endif