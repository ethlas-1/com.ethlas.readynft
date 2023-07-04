using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class ReadyNFTMetaData
{
    [Preserve]
    [JsonProperty("country")]
    public string country { get; set; } = "default_country";

    [Preserve]
    [JsonProperty("deviceId")]
    public string deviceId { get; set; } = "default_deviceId";

    [Preserve]
    [JsonProperty("deviceBrand")]
    public string deviceBrand { get; set; } = "default_deviceBrand";

    [Preserve]
    [JsonProperty("deviceManufacturer")]
    public string deviceManufacturer { get; set; } = "default_deviceManufacturer";

    [Preserve]
    [JsonProperty("deviceModel")]
    public string deviceModel { get; set; } = "default_deviceModel";

    [Preserve]
    [JsonProperty("osName")]
    public string osName { get; set; } = "default_osName";

    [Preserve]
    [JsonProperty("osVersion")]
    public string osVersion { get; set; } = "default_osVersion";

    public ReadyNFTMetaData()
    {
        // Default constructor

    }

    public ReadyNFTMetaData(string _country, string _deviceId, string _deviceBrand, string _deviceManufacturer, string _deviceModel, string _osName, string _osVersion)
    {
        country = _country;
        deviceId = _deviceId;
        deviceBrand = _deviceBrand;
        deviceManufacturer = _deviceManufacturer;
        deviceModel = _deviceModel;
        osName = _osName;
        osVersion = _osVersion;
    }
}

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

    public ReadyNFTSpriteObject()
    {
        // Default constructor
    }
}

[Preserve]
[Serializable]
public class FetchSpritesRequestData: ReadyNFTMetaData
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

    public FetchSpritesRequestData(string _gameId, ReadyNFTMetaData _readyNFTMetaData) {
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
// sprites end

// owned NFTs start
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
public class FetchOwnedNFTsRequestData: ReadyNFTMetaData
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

    public FetchOwnedNFTsRequestData(string _gameId, string _email, ReadyNFTMetaData _readyNFTMetaData) {
        gameId = _gameId;
        email = _email;
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
// owned NFTs end

// download progress report
[Preserve]
[Serializable]
public class ReadyNFTDownloadReport
{
    [Preserve]
    [JsonProperty("percent")]
    public float percent { get; set; }

    [Preserve]
    [JsonProperty("total")]
    public int total { get; set; }

    [Preserve]
    [JsonProperty("current")]
    public int current { get; set; }
    public ReadyNFTDownloadReport(float percent, int total, int current)
    {
        this.percent = percent;
        this.total = total;
        this.current = current;
    }
}
// download progress report end