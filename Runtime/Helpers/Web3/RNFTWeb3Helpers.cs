using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;


public static class RNFTWeb3Helpers
{

    // function to mint a gasless nft
    public static async Task<bool> MintNFT(string walletAddress, string nftId)
    {

        if (string.IsNullOrEmpty(walletAddress))
        {
            Debug.Log("MintNFT: Custodial wallet address not set!");
            return false;
        }

        if (string.IsNullOrEmpty(nftId))
        {
            Debug.Log("MintNFT: NFT ID not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_MINT_NFT_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTGaslessMintRequest requestData = new RNFTGaslessMintRequest(walletAddress, nftId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTGaslessMintResponse result = JsonConvert.DeserializeObject<RNFTGaslessMintResponse>(responseString);
                    return true;
                }
                else
                {
                    Debug.Log("MintNFT: Request failed: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("MintNFT: Request failed: " + e.Message);
            }
        }

        return false;
    }

    // a variation of the gasless mint function that takes a call back function and executes it after the mint is successful
    public static async Task<bool> MintNFTWithCallback(string walletAddress, string nftId, Action<bool> callback)
    {

        if (string.IsNullOrEmpty(walletAddress))
        {
            Debug.Log("MintNFT: Custodial wallet address not set!");
            return false;
        }

        if (string.IsNullOrEmpty(nftId))
        {
            Debug.Log("MintNFT: NFT ID not set!");
            return false;
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_MINT_NFT_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTGaslessMintRequest requestData = new RNFTGaslessMintRequest(walletAddress, nftId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTGaslessMintResponse result = JsonConvert.DeserializeObject<RNFTGaslessMintResponse>(responseString);
                    callback(true);
                    return true;
                }
                else
                {
                    Debug.Log("MintNFT: Request failed: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("MintNFT: Request failed: " + e.Message);
            }
        }

        callback(false);
        return false;
    }
}

