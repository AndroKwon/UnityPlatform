#if UNITY_EDITOR || UNITY_ANDROID
using Jin.PlatformSDK.Unity;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Device
{
    public class PlatformDeviceData
    {
        public static string INITIALIZE_SCHEME = "andro://initialize";
        public static string GET_LANGUAGE_SCHEME = "andro://getLanguage";
        public static string GET_COUNTRY_SCHEME = "andro://getCountry";
        public static string GET_DEVICE_STORAGE_SPACE_SCHEME = "andro://getDeviceStorageSpace";

    }
}
#endif