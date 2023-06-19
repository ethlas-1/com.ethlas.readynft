using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class ReadyNFT
{
    private string apiKey;
    private string gameId;
    private string API_ENDPOINTS_ROOT_URL = "https://3caea960hb.execute-api.ap-southeast-1.amazonaws.com/prod";

    // Initialization method to store the API key
    public void Init(string _apiKey, string _gameId)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            Debug.Log("Invalid API key provided!");
            return;
        }
        if (string.IsNullOrEmpty(_gameId))
        {
            Debug.Log("Invalid Game ID provided!");
            return;
        }
        apiKey = _apiKey;
        gameId = _gameId;
    }


    public async Task<List<ReadyNFTSpriteObject>> FetchSpritesAsync()
    {

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.Log("FetchSpritesAsync: API key not set!");
            return new List<ReadyNFTSpriteObject>();
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("FetchSpritesAsync: Game ID not set!");
            return new List<ReadyNFTSpriteObject>();
        }

        string url = API_ENDPOINTS_ROOT_URL + "/readyNFT/fetchSprites";
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var requestData = new {
                    gameId
                };
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    FetchSpritesResponse result = JsonConvert.DeserializeObject<FetchSpritesResponse>(responseString);
                    return result.data.sprites;
                }
                else
                {
                    Debug.Log("FetchSpritesAsync Request failed: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("FetchSpritesAsync Request failed: " + e.Message);
            }
        }

        return new List<ReadyNFTSpriteObject>(); // return empty list if request fails
    }

    public async Task<List<ReadyNFTOwnedNFTObject>> FetchOwnedNFTsAsync(string email)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.Log("FetchOwnedNFTsAsync: API key not set!");
            return new List<ReadyNFTOwnedNFTObject>();
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("FetchOwnedNFTsAsync: Game ID not set!");
            return new List<ReadyNFTOwnedNFTObject>();
        }

        if (string.IsNullOrEmpty(email))
        {
            Debug.Log("FetchOwnedNFTsAsync: Email not set!");
            return new List<ReadyNFTOwnedNFTObject>();
        }

        string url = API_ENDPOINTS_ROOT_URL + "/readyNFT/fetchOnChainNFTsFromEmail";
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var requestData = new
                {
                    email,
                    gameId
                };
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    FetchOwnedNFTsResponse result = JsonConvert.DeserializeObject<FetchOwnedNFTsResponse>(responseString);
                    Debug.Log("result.data.nfts: " + result.data.nfts);
                    return result.data.nfts;
                }
                else
                {
                    Debug.Log("FetchOwnedNFTsAsync: Request failed: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("FetchOwnedNFTsAsync: Request failed: " + e.Message);
            }
        }

        return new List<ReadyNFTOwnedNFTObject>(); // return empty list if request fails
    }


    // Get the API key
    public string GetApiKey()
    {
        return apiKey;
    }

    // Get the Game ID
    public string GetGameId()
    {
        return gameId;
    }
}
