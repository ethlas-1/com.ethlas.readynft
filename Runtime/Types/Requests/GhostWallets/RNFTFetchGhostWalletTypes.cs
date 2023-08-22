using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTFetchGhostWalletRequest
{
	// euid
	[Preserve]
	[JsonProperty("euid")]
	public string euid { get; set; } = "";

	// game_id
	[Preserve]
	[JsonProperty("game_id")]
	public string game_id { get; set; } = "";

	// default constructor
	public RNFTFetchGhostWalletRequest()
	{
		// Default constructor
	}

	// constructor
	public RNFTFetchGhostWalletRequest(string _euid, string _game_id)
	{
		euid = _euid;
		game_id = _game_id;
	}
}

[Preserve]
[Serializable]
public class RNFTFetchGhostWalletResponseData
{
	// euid 
	[Preserve]
	[JsonProperty("euid")]
	public string euid { get; set; } = "";


	// wallet address
	[Preserve]
	[JsonProperty("walletAddress")]
	public string walletAddress { get; set; } = "";

	// default constructor
	public RNFTFetchGhostWalletResponseData()
	{
		// Default constructor
	}

	// constructor
	public RNFTFetchGhostWalletResponseData(string _euid, string _walletAddress)
	{
		euid = _euid;
		walletAddress = _walletAddress;
	}
}

[Preserve]
[Serializable]
public class RNFTFetchGhostWalletResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public RNFTFetchGhostWalletResponseData data { get; set; } = new RNFTFetchGhostWalletResponseData();

	// default constructor
	public RNFTFetchGhostWalletResponse()
	{
		// Default constructor
	}

	// constructor
	public RNFTFetchGhostWalletResponse(RNFTFetchGhostWalletResponseData _data)
	{
		data = _data;
	}
}



