using Jin.PlatformSDK.Common.Auth;
using Jin.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Jin.PlatformSDK.Unity
{
    public class AuthDelegateManager
    {
        public delegate void OnReceiveMessage(Message json);

        static private Dictionary<string, OnReceiveMessage> _messageDicData = new Dictionary<string, OnReceiveMessage>();
        
        static public bool AddDelegate(string scheme, OnReceiveMessage onReceiveMessage)
        {
            if (_messageDicData.ContainsKey(scheme))
                return false;

            _messageDicData.Add(scheme, onReceiveMessage);
            return true;
        }

        static public bool MessageDelegate(string scheme, Message message)
        {
            if (_messageDicData.ContainsKey(scheme) == false)
            {
                Debug.Log($"{scheme} not initialized");
                return false;
            }

            if (string.IsNullOrEmpty(message.error) == false)
            {
                var error = JsonConvert.DeserializeObject<Error>(message.error);
                Debug.Log($"{scheme} on error : {error.code} - {error.message}");
            }

            _messageDicData[scheme].Invoke(message);
            return true;
        }
    }
}