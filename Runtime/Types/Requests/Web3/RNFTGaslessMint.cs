using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTGaslessMintRequest
{
    // address refers to the wallet address of the user
    [Preserve]
    [JsonProperty("address")]
    public string address { get; set; } = "";

    [Preserve]
    [JsonProperty("nftId")]
    public string nftId { get; set; } = "";

    public RNFTGaslessMintRequest()
    {
        // Default constructor
    }

    public RNFTGaslessMintRequest(string _address, string _nftId)
    {
        address = _address;
        nftId = _nftId;
    }
}

[Preserve]
[Serializable]
public class RNFTGaslessMintTxnReceipt
{
    [Preserve]
    [JsonProperty("to")]
    public string to { get; set; } = "";

    [Preserve]
    [JsonProperty("from")]
    public string from { get; set; } = "";

    [Preserve]
    [JsonProperty("contractAddress")]
    public string contractAddress { get; set; } = "";

    [Preserve]
    [JsonProperty("blockHash")]
    public string blockHash { get; set; } = "";

    [Preserve]
    [JsonProperty("transactionHash")]
    public string transactionHash { get; set; } = "";

    [Preserve]
    [JsonProperty("blockNumber")]
    public int blockNumber { get; set; }

    [Preserve]
    [JsonProperty("status")]
    public int? status { get; set; }
}

[Preserve]
[Serializable]
public class RNFTGaslessMintTxnDetails
{
    [Preserve]
    [JsonProperty("receipt")]
    public RNFTGaslessMintTxnReceipt receipt { get; set; } = new RNFTGaslessMintTxnReceipt();

    public RNFTGaslessMintTxnDetails()
    {
        // Default constructor
    }
}

[Preserve]
[Serializable]
public class RNFTGaslessMintResponse
{
    [Preserve]
    [JsonProperty("data")]
    public RNFTGaslessMintTxnDetails data { get; set; } = new RNFTGaslessMintTxnDetails();

    public RNFTGaslessMintResponse()
    {
        // Default constructor
    }
}
