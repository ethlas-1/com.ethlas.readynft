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

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_GHOST_WALLET_EXISTS_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTDoesGhostWalletExistRequest requestData = new RNFTDoesGhostWalletExistRequest(euid, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

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
}