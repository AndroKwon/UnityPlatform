#if UNITY_EDITOR || UNITY_ANDROID
using Jin.PlatformSDK.Unity;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Device
{
    public class PlatformDevice
    {
        public static string INITIALIZE_SCHEME = "andro://initialize";
        public static string GET_LANGUAGE_SCHEME = "andro://getLanguage";
        public static string GET_COUNTRY_SCHEME = "andro://getCountry";
        public static string GET_DEVICE_STORAGE_SPACE_SCHEME = "andro://getDeviceStorageSpace";

        private static IPlatformDevice _platformDevice;

        public void initialize()
        {

#if UNITY_ANDROID
            _platformDevice = new PlatformAndroidDevice();
#elif UNITY_IOS
            // _platformDevice = new PlatformIOSDevice();
#else
            // _platformDevice = new PlatformEditorDevice();
#endif

        }

        public void InitializeDevice()
        {
            Debug.Log($"Andro_Platform Send {PlatformDevice.INITIALIZE_SCHEME}");

            if (_platformDevice == null)
                return;

            _platformDevice.Initialize();
        }

        public string GetLanguage()
        {
            Debug.Log($"Andro_Platform Send {PlatformDevice.GET_LANGUAGE_SCHEME}");

            if (_platformDevice == null)
                return string.Empty;

            return _platformDevice.GetLanguage();
        }

        public string GetCountry()
        {
            Debug.Log($"Andro_Platform Send {PlatformDevice.GET_COUNTRY_SCHEME}");

            if (_platformDevice == null)
                return string.Empty;

            return _platformDevice.GetCountry();
        }

        public string GetDeviceStorageSpace()
        {
            Debug.Log($"Andro_Platform Send {PlatformDevice.GET_DEVICE_STORAGE_SPACE_SCHEME}");

            if (_platformDevice == null)
                return string.Empty;

            return _platformDevice.GetDeviceStorageSpace();
        }
    }
}
#endif