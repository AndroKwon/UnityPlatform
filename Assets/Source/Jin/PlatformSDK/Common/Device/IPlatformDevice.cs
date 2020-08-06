#if UNITY_EDITOR || UNITY_ANDROID

namespace Jin.PlatformSDK.Common.Device
{
    public interface IPlatformDevice
    {
        string Initialize();
        string GetLanguage();
        string GetCountry();
        string GetDeviceStorageSpace();
    }
}
#endif