﻿using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class RNFTAuthManager : MonoBehaviour
{
    public static string CachePath;

    private const string AppClientID = RNFTAuthConfig.AppClientID; 
    private const string AuthCognitoDomainPrefix = RNFTAuthConfig.AuthCognitoDomainPrefix; 
    private const string RedirectUrl = RNFTAuthConfig.RedirectUrl;
    private const string Region = RNFTAuthConfig.Region; 

    private const string AuthCodeGrantType = "authorization_code";
    private const string RefreshTokenGrantType = "refresh_token";
    private const string CognitoAuthUrl = ".auth." + Region + ".amazoncognito.com";
    private const string TokenEndpointPath = "/oauth2/token";

    private static string _userid = "";

    public async Task<bool> ExchangeAuthCodeForAccessToken(string rawUrlWithGrantCode)
    {
        // Debug.Log("rawUrlWithGrantCode: " + rawUrlWithGrantCode);

        // raw url looks like https://somedomain.com/?code=c91d8bf4-1cb6-46e5-b43a-8def466f3c55
        string allQueryParams = rawUrlWithGrantCode.Split('?')[1];

        // it's likely there won't be more than one param
        string[] paramsSplit = allQueryParams.Split('&');

        foreach (string param in paramsSplit)
        {
            // Debug.Log("param: " + param);

            // find the code parameter and its value
            if (param.StartsWith("code"))
            {
                string grantCode = param.Split('=')[1];
                string grantCodeCleaned = grantCode.removeAllNonAlphanumericCharsExceptDashes(); // sometimes the url has a # at the end of the string
                Debug.Log("The CLEANED GRANT CODE is " + grantCode);
                return await CallCodeExchangeEndpoint(grantCodeCleaned);
            }
            else
            {
                Debug.Log("Code not found");
            }
        }
        return false;
    }

    // exchanges grant code for tokens
    private async Task<bool> CallCodeExchangeEndpoint(string grantCode)
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", AuthCodeGrantType);
        form.AddField("client_id", AppClientID);
        form.AddField("code", grantCode);
        form.AddField("redirect_uri", RedirectUrl);

        // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/token-endpoint.html
        string requestPath = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl + TokenEndpointPath;

        UnityWebRequest webRequest = UnityWebRequest.Post(requestPath, form);
        await webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Code exchange failed: " + webRequest.error + "\n" + webRequest.result + "\n" + webRequest.responseCode);
            webRequest.Dispose();
        }
        else
        {
            Debug.Log("Success, Code exchange complete!");

            RNFTAuthResultType authenticationResultType = JsonUtility.FromJson<RNFTAuthResultType>(webRequest.downloadHandler.text);
            // Debug.Log("ID token: " + authenticationResultType.id_token);

            _userid = RNFTAuthUtilities.GetUserSubFromIdToken(authenticationResultType.id_token);
            Debug.Log("THE USER ID OBTAINED FOR THIS USER IS " + _userid);

            // update session cache
            RNFTSaveDataManager.SaveJsonData(new RNFTUserSessionCache(authenticationResultType, _userid));
            webRequest.Dispose();
            return true;
        }
        return false;
    }

    public async Task<bool> CallRefreshTokenEndpoint()
    {
        RNFTUserSessionCache userSessionCache = new RNFTUserSessionCache();
        RNFTSaveDataManager.LoadJsonData(userSessionCache);

        string preservedRefreshToken = "";

        if (userSessionCache != null && userSessionCache._refreshToken != null && userSessionCache._refreshToken != "")
        {
            // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/token-endpoint.html
            string refreshTokenUrl = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl + TokenEndpointPath;
            // Debug.Log(refreshTokenUrl);

            preservedRefreshToken = userSessionCache._refreshToken;

            WWWForm form = new WWWForm();
            form.AddField("grant_type", RefreshTokenGrantType);
            form.AddField("client_id", AppClientID);
            form.AddField("refresh_token", userSessionCache._refreshToken);

            UnityWebRequest webRequest = UnityWebRequest.Post(refreshTokenUrl, form);
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            await webRequest.SendWebRequest();


            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Refresh token call failed: " + webRequest.error + "\n" + webRequest.result + "\n" + webRequest.responseCode);
                // clear out invalid user session data to force re-authentication
                ClearUserSessionData();
                webRequest.Dispose();
            }
            else
            {
                Debug.Log("Success, Refresh token call complete!");
                // Debug.Log(webRequest.downloadHandler.text);

                RNFTAuthResultType authenticationResultType = JsonUtility.FromJson<RNFTAuthResultType>(webRequest.downloadHandler.text);

                // token endpoint to get refreshed access token does NOT return the refresh token, so manually save it from before.
                authenticationResultType.refresh_token = preservedRefreshToken;

                _userid = RNFTAuthUtilities.GetUserSubFromIdToken(authenticationResultType.id_token);

                // update session cache
                RNFTSaveDataManager.SaveJsonData(new RNFTUserSessionCache(authenticationResultType, _userid));
                webRequest.Dispose();
                return true;
            }
        }
        return false;
    }

    // Revokes refresh token and any access tokens issued from the refresh token.  Forces user to re-authenticate.
    private async Task<bool> RevokeRefreshToken()
    {
        RNFTUserSessionCache userSessionCache = new RNFTUserSessionCache();
        RNFTSaveDataManager.LoadJsonData(userSessionCache);

        if (userSessionCache != null && userSessionCache._refreshToken != null && userSessionCache._refreshToken != "")
        {
            // DOCS (WARNING these docs are not accurate at the time of this implementation): https://docs.aws.amazon.com/cognito/latest/developerguide/revocation-endpoint.html
            // These were more accurate: https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_RevokeToken.html
            // Also, the Enable token revocation option must be enabled for this to work under User Pool -> App Clients tab.
            string revokeTokenEndpoint = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl + "/oauth2/revoke";
            // Debug.Log(revokeTokenEndpoint);

            WWWForm form = new WWWForm();
            form.AddField("client_id", AppClientID);
            form.AddField("token", userSessionCache._refreshToken);

            UnityWebRequest webRequest = UnityWebRequest.Post(revokeTokenEndpoint, form);
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Revoke token call failed: " + webRequest.error + "\n" + webRequest.result + "\n" + webRequest.responseCode);
                webRequest.Dispose();
            }
            else
            {
                Debug.Log("Success, Revoke token call complete!");
                webRequest.Dispose();
                return true;
            }
        }
        return false;
    }

    public async void Logout()
    {
        bool logoutSuccess = await RevokeRefreshToken();

        // Important! Make sure to remove the local stored tokens.
        ClearUserSessionData();
        Debug.Log("user logged out.");
    }

    // Saves an empty user session object that will clear out all locally saved tokens.
    private void ClearUserSessionData()
    {
        RNFTUserSessionCache userSessionCache = new RNFTUserSessionCache();
        RNFTSaveDataManager.SaveJsonData(userSessionCache);
    }

    public string GetUsersId()
    {
        // Debug.Log("GetUserId: [" + _userid + "]");
        if (_userid == null || _userid == "")
        {
            // load userid from cached session 
            RNFTUserSessionCache userSessionCache = new RNFTUserSessionCache();
            RNFTSaveDataManager.LoadJsonData(userSessionCache);
            _userid = userSessionCache.getUserId();
        }
        return _userid;
    }

    public string GetIdToken()
    {
        RNFTUserSessionCache userSessionCache = new RNFTUserSessionCache();
        RNFTSaveDataManager.LoadJsonData(userSessionCache);
        return userSessionCache.getIdToken();
    }

    public string GetUserId()
    {
        RNFTUserSessionCache userSessionCache = new RNFTUserSessionCache();
        RNFTSaveDataManager.LoadJsonData(userSessionCache);
        return userSessionCache.getUserId();
    }

    public string GetLoginUrl()
    {
        // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/login-endpoint.html
        string loginUrl = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl
           + "/login?response_type=code&client_id="
           + AppClientID + "&redirect_uri=" + RedirectUrl;
        return loginUrl;
    }

    void Awake()
    {
        Debug.Log("[RNFT] Deep Link Manager Awake!");
        CachePath = Application.persistentDataPath;
    }
}
