using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class RNFTGhostWalletHelpers
{
    // function to check if a ghost walelt exists or not
    public static async Task<bool> DoesGhostWalletExist(string euid, string gameId)
    {
        // ensure that the euid and game id are not empty
        if (string.IsNullOrEmpty(euid))
        {
            Debug.Log("DoesGhostWalletExist: EUID not set!");
            return false;
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("DoesGhostWalletExist: Game ID not set!");
            return false;
        }

        string apiKey = RNFTAuthManager.Instance?.apiKey;
        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_GHOST_WALLET_EXISTS_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTDoesGhostWalletExistRequest requestData = new RNFTDoesGhostWalletExistRequest(euid, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                // Add the header
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTDoesGhostWalletExistResponse result = JsonConvert.DeserializeObject<RNFTDoesGhostWalletExistResponse>(responseString);
                    return result.data.exists;
                }
                else
                {
                    Debug.Log("DoesGhostWalletExist: Request failed: " + response.StatusCode);
                }

            }
            catch (Exception e)
            {
                Debug.Log("DoesGhostWalletExist: Request failed: " + e.Message);
            }
        }

        return false;
    }

    // function to fetch ghost wallet 
    public static async Task<string> FetchGhostWallet(string euid, string gameId)
    {
        // ensure that the euid and game id are not empty
        if (string.IsNullOrEmpty(euid))
        {
            Debug.Log("FetchGhostWallet: EUID not set!");
            return null;
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("FetchGhostWallet: Game ID not set!");
            return null;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_FETCH_GHOST_WALLET_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTFetchGhostWalletRequest requestData = new RNFTFetchGhostWalletRequest(euid, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTFetchGhostWalletResponse result = JsonConvert.DeserializeObject<RNFTFetchGhostWalletResponse>(responseString);
                    return result.data.walletAddress;
                }
                else
                {
                    Debug.Log("FetchGhostWallet: Request failed: " + response.StatusCode);
                }

            }
            catch (Exception e)
            {
                Debug.Log("FetchGhostWallet: Request failed: " + e.Message);
            }
        }

        // default return value
        return "";
    }

    // function to transfer ghost wallet ownership to a rnft user
    public static async Task<bool> TrfGhostWalletToRNFTUser(string euid, string uuid)
    {
        // ensure that the euid and uuid are not empty
        if (string.IsNullOrEmpty(euid))
        {
            Debug.Log("TrfGhostWalletToRNFTUser: EUID not set!");
            return false;
        }

        if (string.IsNullOrEmpty(uuid))
        {
            Debug.Log("TrfGhostWalletToRNFTUser: UUID not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_TRF_GHOST_WALLET_TO_RNFT_USER_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTTrfGWToRNFTUserRequest requestData = new RNFTTrfGWToRNFTUserRequest(euid, uuid);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTTrfGWToRNFTUserResponse result = JsonConvert.DeserializeObject<RNFTTrfGWToRNFTUserResponse>(responseString);
                    return result.data.transferred;
                }
                else
                {
                    Debug.Log("TrfGhostWalletToRNFTUser: Request failed: " + response.StatusCode);
                }

            }
            catch (Exception e)
            {
                Debug.Log("TrfGhostWalletToRNFTUser: Request failed: " + e.Message);
            }
        }

        // default return value
        return false;
    }

    // function to login a ghost wallet
    public static async Task<bool> GhostWalletLogin(string euid, string gameId)
    {
        // ensure that the euid and game id are not empty
        if (string.IsNullOrEmpty(euid))
        {
            Debug.Log("GhostWalletLogin: EUID not set!");
            return false;
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("GhostWalletLogin: Game ID not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_GHOST_WALLET_LOGIN_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTGhostWalletLoginRequest requestData = new RNFTGhostWalletLoginRequest(euid, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTGhostWalletLoginResponse result = JsonConvert.DeserializeObject<RNFTGhostWalletLoginResponse>(responseString);
                    return result.data.loggedIn;
                }
                else
                {
                    Debug.Log("GhostWalletLogin: Request failed: " + response.StatusCode);
                }

            }
            catch (Exception e)
            {
                Debug.Log("GhostWalletLogin: Request failed: " + e.Message);
            }
        }

        // default return value
        return false;
    }

    // function to login a ghost wallet - with a callback
    public static async Task<bool> GhostWalletLoginWithCallback(string euid, string gameId, Action<bool> callback)
    {
        // ensure that the euid and game id are not empty
        if (string.IsNullOrEmpty(euid))
        {
            Debug.Log("GhostWalletLogin: EUID not set!");
            return false;
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("GhostWalletLogin: Game ID not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_GHOST_WALLET_LOGIN_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTGhostWalletLoginRequest requestData = new RNFTGhostWalletLoginRequest(euid, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTGhostWalletLoginResponse result = JsonConvert.DeserializeObject<RNFTGhostWalletLoginResponse>(responseString);
                    callback(result.data.loggedIn);
                    return result.data.loggedIn;
                }
                else
                {
                    Debug.Log("GhostWalletLogin: Request failed: " + response.StatusCode);
                }

            }
            catch (Exception e)
            {
                Debug.Log("GhostWalletLogin: Request failed: " + e.Message);
            }
        }

        // default return value
        callback(false);
        return false;
    }

    // function to trigger a transfer wallet address lambda
    public static async Task<bool> BindAccount(string from, string to, string toType)
    {
        // ensure that the from and game id are not empty
        if (string.IsNullOrEmpty(from))
        {
            Debug.Log("BindAccount: from not set!");
            return false;
        }

        if (string.IsNullOrEmpty(to))
        {
            Debug.Log("BindAccount: to not set!");
            return false;
        }

        if (string.IsNullOrEmpty(toType))
        {
            Debug.Log("BindAccount: toType not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_BIND_ACCOUNT;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTBindAccountRequest requestData = new RNFTBindAccountRequest(from, to, toType);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Debug.Log("BindAccount: Request failed: " + response.StatusCode);
                }
                
            }
            catch (Exception e)
            {
                Debug.Log("BindAccount: Request failed: " + e.Message);
            }
        }

        return false;
    }

    // function to login a ghost wallet
    public static async Task<bool> WalletLogin(string uuid, string gameId)
    {
        // ensure that the uuid and game id are not empty
        if (string.IsNullOrEmpty(uuid))
        {
            Debug.Log("WalletLogin: uuid not set!");
            return false;
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("WalletLogin: Game ID not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_WALLET_LOGIN_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTWalletLoginRequest requestData = new RNFTWalletLoginRequest(uuid, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTWalletLoginResponse result = JsonConvert.DeserializeObject<RNFTWalletLoginResponse>(responseString);
                    return result.data.loggedIn;
                }
                else
                {
                    Debug.Log("WalletLogin: Request failed: " + response.StatusCode);
                }

            }
            catch (Exception e)
            {
                Debug.Log("WalletLogin: Request failed: " + e.Message);
            }
        }

        // default return value
        return false;
    }
}