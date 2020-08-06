using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformManagerTest : MonoBehaviour
{
    public Button FacebookButton;
    public Button GoogleButton;

    public Button LogoutButton;

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        Jin.PlatformSDK.Common.Auth.PlatformAuth.AuthInitialize("facebook");
        Jin.PlatformSDK.Common.Auth.PlatformAuth.AuthInitialize("google");
        
        FacebookButton.onClick.AddListener(() => FacebookLogin());
        GoogleButton.onClick.AddListener(() => GoogleLogin());

        LogoutButton.onClick.AddListener(() => Logout());

        string provider = PlayerPrefs.GetString("LastLoginProvider", string.Empty);

        if (string.IsNullOrEmpty(provider) == false)
        {
            Jin.PlatformSDK.Common.Auth.PlatformAuth.Login(provider, (error, token) =>
            {
                text.text = $"{provider} login : {token}";
                Debug.Log($"{provider} login : {token}");
            });
        }
    }

    private void GoogleLogin()
    {
        var resultLogout = Jin.PlatformSDK.Common.Auth.PlatformAuth.IsLogin();
        if (string.IsNullOrEmpty(resultLogout) == false)
        {
            text.text = $"{resultLogout} 로그아웃 필요";
            return;
        }

        text.text = "Google 로그인 시도";

        Jin.PlatformSDK.Common.Auth.PlatformAuth.Login("google", (error, token) =>
        {
            text.text = $"google login : {token}";
            Debug.Log($"google login : {token}");

            PlayerPrefs.SetString("LastLoginProvider", "google");
            PlayerPrefs.Save();
        });
    }
    
    private void FacebookLogin()
    {
        var resultLogout = Jin.PlatformSDK.Common.Auth.PlatformAuth.IsLogin();
        if (string.IsNullOrEmpty(resultLogout) == false)
        {
            text.text = $"{resultLogout} 로그아웃 필요";
            return;
        }

        text.text = "Facebook 로그인 시도";

        Jin.PlatformSDK.Common.Auth.PlatformAuth.Login("facebook", (error, token) =>
        {
            text.text = $"facebook login : {token}";
            Debug.Log($"facebook login : {token}");

            PlayerPrefs.SetString("LastLoginProvider", "facebook");
            PlayerPrefs.Save();
        });
    }
    
    private void Logout()
    {
        Jin.PlatformSDK.Common.Auth.PlatformAuth.Logout((error, provider) =>
        {
            text.text = $"logout : {provider}";

            PlayerPrefs.SetString("LastLoginProvider", string.Empty);
            PlayerPrefs.Save();
        });
    }
}
