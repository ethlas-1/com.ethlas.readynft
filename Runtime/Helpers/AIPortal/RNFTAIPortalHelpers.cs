using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RNFTAIPortalHelpers
{
    // takes player uuids as one of the arguments which is a list of strings
    public async Task<List<RNFTAIPortalAssetObject>> FetchAIPortalAssets(List<string> playerUuids)
    {
        string apiKey = RNFTManager.Instance?.apiKey;
        string gameId = RNFTManager.Instance?.gameId;

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.Log("FetchAIPortalAssets: API key not set!");
            return new List<RNFTAIPortalAssetObject>();
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("FetchAIPortalAssets: Game ID not set!");
            return new List<RNFTAIPortalAssetObject>();
        }

        if (playerUuids.Count == 0)
        {
            Debug.Log("FetchAIPortalAssets: No player uuids provided!");
            return new List<RNFTAIPortalAssetObject>();
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_AI_PORTAL_ROUTE + RNFTRequestsConfig.API_FETCH_AI_PORTAL_ASSETS_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                RNFTFetchAIPortalAssetsRequest requestData = new RNFTFetchAIPortalAssetsRequest(playerUuids, gameId);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                // Add the header
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    RNFTFetchAIPortalAssetsResponse result = JsonConvert.DeserializeObject<RNFTFetchAIPortalAssetsResponse>(responseString);
                    return result.data.assets;
                }
                else
                {
                    Debug.Log("FetchAIPortalAssets Request failed - not succcessful: " + response.StatusCode);
                    Debug.Log(response);
                }
            }
            catch (Exception e)
            {
                Debug.Log("FetchAIPortalAssets: Exception: " + e.Message);
                Debug.Log(e);
            }

        }

        return new List<RNFTAIPortalAssetObject>(); // return an empty list if the request fails

    }
}