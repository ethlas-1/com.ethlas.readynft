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