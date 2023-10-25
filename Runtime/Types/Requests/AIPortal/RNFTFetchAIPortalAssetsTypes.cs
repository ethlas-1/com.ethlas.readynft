using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTFetchAIPortalAssetsRequest
{
    // player uuids 
    [Preserve]
    [JsonProperty("playerUuids")]
    public List<string> playerUuids { get; set; } = new List<string>();

    // game id 
    [Preserve]
    [JsonProperty("gameId")]
    public string gameId { get; set; } = "";

    // default constructor
    public RNFTFetchAIPortalAssetsRequest()
    {
        // Default constructor
    }

    // constructor
    public RNFTFetchAIPortalAssetsRequest(List<string> _playerUuids, string _gameId)
    {
        playerUuids = _playerUuids;
        gameId = _gameId;
    }
}

[Preserve]
[Serializable]
public class RNFTAIPortalAssetObject
{
    // asset id 
    [Preserve]
    [JsonProperty("assetId")]
    public string assetId { get; set; } = "";

    // player uuid 
    [Preserve]
    [JsonProperty("playerUuid")]
    public string playerUuid { get; set; } = "";

    // game id 
    [Preserve]
    [JsonProperty("gameId")]
    public string gameId { get; set; } = "";

    // asset type
    [Preserve]
    [JsonProperty("assetType")]
    public string assetType { get; set; } = "";

    // image url 
    [Preserve]
    [JsonProperty("imageUrl")]
    public string imageUrl { get; set; } = "";

    // version
    [Preserve]
    [JsonProperty("version")]
    public int version { get; set; } = 0;

    // default constructor
    public RNFTAIPortalAssetObject()
    {
        // Default constructor
    }

    // constructor
    public RNFTAIPortalAssetObject(string _assetId, string _playerUuid, string _gameId, string _assetType, string _imageUrl, int _version)
    {
        assetId = _assetId;
        playerUuid = _playerUuid;
        gameId = _gameId;
        assetType = _assetType;
        imageUrl = _imageUrl;
        version = _version;
    }
}

[Preserve]
[Serializable]
class RNFTFetchAiPortalAssetsResponseData
{
    // assets
    [Preserve]
    [JsonProperty("assets")]
    public List<RNFTAIPortalAssetObject> assets { get; set; } = new List<RNFTAIPortalAssetObject>();

    // default constructor
    public RNFTFetchAiPortalAssetsResponseData()
    {
        // Default constructor
    }

    // constructor
    public RNFTFetchAiPortalAssetsResponseData(List<RNFTAIPortalAssetObject> _assets)
    {
        assets = _assets;
    }
}

[Preserve]
[Serializable]
class RNFTFetchAIPortalAssetsResponse
{
    // message
    [Preserve]
    [JsonProperty("message")]
    public string message { get; set; }

    // data
    [Preserve]
    [JsonProperty("data")]
    public RNFTFetchAiPortalAssetsResponseData data { get; set; } = new RNFTFetchAiPortalAssetsResponseData();

    // default constructor
    public RNFTFetchAIPortalAssetsResponse()
    {
        // Default constructor
    }

    // constructor
    public RNFTFetchAIPortalAssetsResponse(string _message, RNFTFetchAiPortalAssetsResponseData _data)
    {
        message = _message;
        data = _data;
    }

}