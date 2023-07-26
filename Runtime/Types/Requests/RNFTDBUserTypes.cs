using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

// sprites start
[Preserve]
[Serializable]
public class RNFTDBUserTypes
{
    // attributes are "uid": "loginMethod" "email" "createdAt" "updatedAt" "custodialWalletAddress"

    [Preserve]
    [JsonProperty("uid")]
    public string uid { get; set; } = "";

    [Preserve]
    [JsonProperty("loginMethod")]
    public string loginMethod { get; set; } = "";

    [Preserve]
    [JsonProperty("email")]
    public string email { get; set; } = "";

    [Preserve]
    [JsonProperty("createdAt")]
    public string createdAt { get; set; } = "";

    [Preserve]
    [JsonProperty("updatedAt")]
    public string updatedAt { get; set; } = "";

    [Preserve]
    [JsonProperty("custodialWalletAddress")]
    public string custodialWalletAddress { get; set; } = "";

    public RNFTDBUserTypes()
    {
        // Default constructor
    }
}

[Preserve]
[Serializable]
public class FetchUserDataFromDBRequestData
{
    [Preserve]
    [JsonProperty("uid")]
    public string uid { get; set; } = "";

    public FetchUserDataFromDBRequestData()
    {
        // Default constructor
    }

    public FetchUserDataFromDBRequestData(string _uid)
    {
        uid = _uid;
    }
}

[Preserve]
[Serializable]
public class FetchUserDataFromDBResponse
{
    [Preserve]
    [JsonProperty("message")]
    public string message { get; set; }

    [Preserve]
    [JsonProperty("data")]
    public RNFTDBUserTypes data { get; set; } = new RNFTDBUserTypes();

    public FetchUserDataFromDBResponse()
    {
        // Default constructor
    }
}