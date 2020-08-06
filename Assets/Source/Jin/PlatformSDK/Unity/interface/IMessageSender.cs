#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
namespace Jin.PlatformSDK.Unity
{
    public interface IMessageSender
    {
        string GetSync(string jsonString);

        void GetAsync(string jsonString);
    }
}
#endif