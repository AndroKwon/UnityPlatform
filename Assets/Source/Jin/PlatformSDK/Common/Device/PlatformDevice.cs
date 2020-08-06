#if UNITY_EDITOR || UNITY_ANDROID
using Jin.PlatformSDK.Unity;
using UnityEngine;

namespace Jin.PlatformSDK.Common.Device
{
    public class PlatformDevice
    {
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
            Debug.Log($"Andro_Platform Send {PlatformDeviceData.INITIALIZE_SCHEME}");

            if (_platformDevice == null)
                return;

            _platformDevice.Initialize();
        }

        public string GetLanguage()
        {
            Debug.Log($"Andro_Platform Send {PlatformDeviceData.GET_LANGUAGE_SCHEME}");

            if (_platformDevice == null)
                return string.Empty;

            return _platformDevice.GetLanguage();
        }

        public string GetCountry()
        {
            Debug.Log($"Andro_Platform Send {PlatformDeviceData.GET_COUNTRY_SCHEME}");

            if (_platformDevice == null)
                return string.Empty;

            return _platformDevice.GetCountry();
        }

        public string GetDeviceStorageSpace()
        {
            Debug.Log($"Andro_Platform Send {PlatformDeviceData.GET_DEVICE_STORAGE_SPACE_SCHEME}");

            if (_platformDevice == null)
                return string.Empty;

            return _platformDevice.GetDeviceStorageSpace();
        }
    }
}
#endif