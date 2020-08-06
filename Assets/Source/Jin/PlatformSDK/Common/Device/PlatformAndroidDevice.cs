#if UNITY_EDITOR || UNITY_ANDROID
using System;
using Jin.PlatformSDK.Unity;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Device
{
    public class PlatformAndroidDevice : IPlatformDevice
    {
        public string Initialize()
        {
            var message = new UnityMessage();

            message.scheme = PlatformDevice.INITIALIZE_SCHEME;

            return AndroidMessageSender.Instance.GetSync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        public string GetLanguage()
        {
            var message = new UnityMessage();

            message.scheme = PlatformDevice.GET_LANGUAGE_SCHEME;

            return AndroidMessageSender.Instance.GetSync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        public string GetCountry()
        {
            var message = new UnityMessage();

            message.scheme = PlatformDevice.GET_COUNTRY_SCHEME;

            return AndroidMessageSender.Instance.GetSync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        public string GetDeviceStorageSpace()
        {
            var message = new UnityMessage();

            message.scheme = PlatformDevice.GET_DEVICE_STORAGE_SPACE_SCHEME;

            return AndroidMessageSender.Instance.GetSync(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }
    }
}
#endif