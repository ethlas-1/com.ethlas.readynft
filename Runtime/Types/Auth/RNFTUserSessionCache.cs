using UnityEngine;

// Use this class to store user session data to use for refreshing tokens
[System.Serializable]
public class RNFTUserSessionCache : ISaveable
{
    public string _idToken;
    public string _refreshToken;
    public string _userId;

    public RNFTUserSessionCache() { }

    public RNFTUserSessionCache(RNFTAuthResultType authenticationResultType, string userId)
    {
        _idToken = authenticationResultType.id_token;
        _refreshToken = authenticationResultType.refresh_token;
        _userId = userId;
    }

    public string getIdToken()
    {
        return _idToken;
    }

    public string getRefreshToken()
    {
        return _refreshToken;
    }

    public string getUserId()
    {
        return _userId;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jsonToLoadFrom)
    {
        JsonUtility.FromJsonOverwrite(jsonToLoadFrom, this);
    }

    public string FileNameToUseForData()
    {
        return "rnft_cache_data.dat";
    }
}
