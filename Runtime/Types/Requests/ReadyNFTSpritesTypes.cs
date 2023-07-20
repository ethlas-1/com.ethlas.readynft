using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

// sprites start
[Preserve]
[Serializable]
public class ReadyNFTSpriteObject
{
    [Preserve]
    [JsonProperty("spriteId")]
    public string spriteId { get; set; } = "";

    [Preserve]
    [JsonProperty("gameId")]
    public string gameId { get; set; } = "";

    [Preserve]
    [JsonProperty("nftId")]
    public string nftId { get; set; } = "";

    [Preserve]
    [JsonProperty("nftName")]
    public string nftName { get; set; } = "";

    [Preserve]
    [JsonProperty("contract")]
    public string contract { get; set; } = "";

    [Preserve]
    [JsonProperty("images")]
    public Dictionary<string, string> images { get; set; } = new Dictionary<string, string>();

    [Preserve]
    [JsonProperty("stats")]
    public Dictionary<string, int> stats { get; set; } = new Dictionary<string, int>();

    [Preserve]
    [JsonProperty("version")]
    public int version { get; set; } = 1;

    public ReadyNFTSpriteObject()
    {
        // Default constructor
    }
}

[Preserve]
[Serializable]
public class FetchSpritesRequestData : ReadyNFTMetaData
{
    [Preserve]
    [JsonProperty("gameId")]
    public string gameId { get; set; } = "";

    public FetchSpritesRequestData()
    {
        // Default constructor
    }

    public FetchSpritesRequestData(string _gameId)
    {
        gameId = _gameId;
    }

    public FetchSpritesRequestData(string _gameId, ReadyNFTMetaData _readyNFTMetaData)
    {
        gameId = _gameId;
        country = _readyNFTMetaData.country;
        deviceId = _readyNFTMetaData.deviceId;
        deviceBrand = _readyNFTMetaData.deviceBrand;
        deviceManufacturer = _readyNFTMetaData.deviceManufacturer;
        deviceModel = _readyNFTMetaData.deviceModel;
        osName = _readyNFTMetaData.osName;
        osVersion = _readyNFTMetaData.osVersion;
    }
}

[Preserve]
[Serializable]
public class FetchSpritesResponse
{
    [Preserve]
    [JsonProperty("message")]
    public string message { get; set; }

    [Preserve]
    [JsonProperty("data")]
    public FetchSpritesData data { get; set; } = new FetchSpritesData();

    public FetchSpritesResponse()
    {
        // Default constructor
    }
}

[Preserve]
[Serializable]
public class FetchSpritesData : ReadyNFTMetaData
{
    [Preserve]
    [JsonProperty("sprites")]
    public List<ReadyNFTSpriteObject> sprites { get; set; } = new List<ReadyNFTSpriteObject>();

    public FetchSpritesData()
    {
        // Default constructor        
    }
}