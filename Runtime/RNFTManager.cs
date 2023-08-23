using UnityEngine;
using System.Collections;

public class RNFTManager: MonoBehaviour
{
    public static RNFTAuthManager Instance { get; private set; }
    public string apiKey;
    public string gameId;

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

    // Set the API key
    public void SetApiKey(string _apiKey)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            Debug.Log("Invalid API key provided!");
            return;
        }
        apiKey = _apiKey;
    }

    // Set the Game ID
    public void SetGameId(string _gameId)
    {
        if (string.IsNullOrEmpty(_gameId))
        {
            Debug.Log("Invalid Game ID provided!");
            return;
        }
        gameId = _gameId;
    }

        void Awake()
    {
        Debug.Log("[RNFT] Auth Manager Awake!");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}