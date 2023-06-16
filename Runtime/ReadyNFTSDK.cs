using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class ReadyNFT
{
    private string apiKey;
    private string gameId;
    private string API_ENDPOINTS_ROOT_URL = "http://localhost:4000/dev";

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
            Debug.Log("API key not set!");
            return new List<ReadyNFTSpriteObject>();
        }

        if (string.IsNullOrEmpty(gameId))
        {
            Debug.Log("Game ID not set!");
            return new List<ReadyNFTSpriteObject>();
        }

        string url = API_ENDPOINTS_ROOT_URL + "/readyNFT/fetchSprites?gameId=" + gameId;
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    FetchSpritesResponse result = JsonConvert.DeserializeObject<FetchSpritesResponse>(responseString);
                    return result.data.sprites;
                }
                else
                {
                    Debug.Log("Request failed: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Request failed: " + e.Message);
            }
        }

        return new List<ReadyNFTSpriteObject>(); // return empty list if request fails
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
