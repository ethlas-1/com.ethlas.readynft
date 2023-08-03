using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTAmplitudeEvent
{
    [Preserve]
    [JsonProperty("country")]
    public string country { get; set; } = "default_country";

    [Preserve]
    [JsonProperty("device_id")]
    public string device_id { get; set; } = "default_deviceId";

    [Preserve]
    [JsonProperty("user_id")]
    public string user_id { get; set; } = "default_userId";

    [Preserve]
    [JsonProperty("device_brand")]
    public string device_brand { get; set; } = "default_deviceBrand";

    [Preserve]
    [JsonProperty("device_manufacturer")]
    public string device_manufacturer { get; set; } = "default_deviceManufacturer";

    [Preserve]
    [JsonProperty("device_model")]
    public string device_model { get; set; } = "default_deviceModel";

    [Preserve]
    [JsonProperty("os_name")]
    public string os_name { get; set; } = "default_osName";

    [Preserve]
    [JsonProperty("os_version")]
    public string os_version { get; set; } = "default_osVersion";

    [Preserve]
    [JsonProperty("event_type")]
    public string event_type { get; set; } = "default_eventType";

    [Preserve]
    [JsonProperty("event_properties")]
    public Dictionary<string, string> event_properties { get; set; } = new Dictionary<string, string>();

    public RNFTAmplitudeEvent()
    {
        // Default constructor

    }
}