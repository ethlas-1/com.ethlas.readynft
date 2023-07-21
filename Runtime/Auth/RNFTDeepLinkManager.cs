using System;
using UnityEngine;

// Handles deep link events into the Unity app on a mobile device.  Not tested or supported on desktop applications.
// Class is linked to ProcessDeepLinkContainer in Unity Editor
// source: https://docs.unity3d.com/Manual/enabling-deep-linking.html

public class RNFTDeepLinkManager : MonoBehaviour
{
    public static RNFTDeepLinkManager Instance { get; private set; }

    private string deeplinkURL;
    private RNFTAuthManager _authManager;

    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        ProcessDeepLink(url);
    }

    private void Awake()
    {
        Debug.Log("[RNFT] Deep Link Manager Awake!");

        if (Instance == null)
        {
            Instance = this;

            // setup deeplink callback
            Application.deepLinkActivated += onDeepLinkActivated;

            if (!String.IsNullOrEmpty(Application.absoluteURL))
            {
                // Debug.Log("deep link: " + Application.absoluteURL);
                onDeepLinkActivated(Application.absoluteURL);
            }
            else
            {
                deeplinkURL = "NONE";
            }

            // we want to reuse this object between scene loading, which makes sure it is available when receiving a deep link url 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // I think this would destroy a new instance that was attempted to be created when one already exists
            Destroy(gameObject);
        }
    }

    public string GetDeepLink()
    {
        return deeplinkURL;
    }

    public async void ProcessDeepLink(string deepLinkUrl)
    {
        Debug.Log("[RNFT] Processing deep link: " + deepLinkUrl);

        // fetching the auth manager from the hierarchy
        _authManager = FindObjectOfType<RNFTAuthManager>();

        // check to ensure that the auth manager is not null
        if (_authManager == null)
        {
            Debug.Log("[RNFT] Auth manager is null, cannot process deep link!");
            return;
        }

        bool exchangeSuccess = await _authManager.ExchangeAuthCodeForAccessToken(deepLinkUrl);

        if (exchangeSuccess)
        {
            Debug.Log("[RNFT] Successfully exchanged the auth code for the access token!");
        } else
        {
            Debug.Log("[RNFT] An error occurred when exchanging the auth code for the access token!");
        }
    }
}