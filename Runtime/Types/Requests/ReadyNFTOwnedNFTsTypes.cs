using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;


[Preserve]
[Serializable]
public class ReadyNFTOwnedNFTObject
{
    [Preserve]
    [JsonProperty("contractAddress")]
    public string contractAddress { get; set; }

    [Preserve]
    [JsonProperty("collectionName")]
    public string collectionName { get; set; }

    [Preserve]
    [JsonProperty("collectionSymbol")]
    public string collectionSymbol { get; set; }

    [Preserve]
    [JsonProperty("tokenId")]
    public string tokenId { get; set; }

    [Preserve]
    [JsonProperty("tokenType")]
    public string tokenType { get; set; }
}

[Preserve]
[Serializable]
public class FetchOwnedNFTsRequestData : ReadyNFTMetaData
{
    [Preserve]
    [JsonProperty("gameId")]
    public string gameId { get; set; } = "";

    [Preserve]
    [JsonProperty("email")]
    public string email { get; set; } = "";

    public FetchOwnedNFTsRequestData()
    {
        // Default constructor
    }

    public FetchOwnedNFTsRequestData(string _gameId, string _email)
    {
        gameId = _gameId;
        email = _email;
    }

    public FetchOwnedNFTsRequestData(string _gameId, string _email, ReadyNFTMetaData _readyNFTMetaData)
    {
        gameId = _gameId;
        email = _email;
        country = _readyNFTMetaData.country;
        deviceId = _readyNFTMetaData.deviceId;
        deviceBrand = _readyNFTMetaData.deviceBrand;
        deviceManufacturer = _readyNFTMetaData.deviceManufacturer;
        deviceModel = _readyNFTMetaData.deviceModel;
        osName = _readyNFTMetaData.osName;
        osVersion = _readyNFTMetaData.osVersion;
        appVersion = _readyNFTMetaData.appVersion;
        bundleIdentifier = _readyNFTMetaData.bundleIdentifier;
    }
}

[Preserve]
[Serializable]
public class FetchOwnedNFTsResponse
{
    [Preserve]
    [JsonProperty("message")]
    public string message { get; set; }

    [Preserve]
    [JsonProperty("data")]
    public FetchOwnedNFTsData data { get; set; }
}

[Preserve]
[Serializable]
public class FetchOwnedNFTsData
{
    [Preserve]
    [JsonProperty("nfts")]
    public List<ReadyNFTOwnedNFTObject> nfts { get; set; }
}
