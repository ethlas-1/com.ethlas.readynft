using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTWalletLoginRequest
{
	// uuid
	[Preserve]
	[JsonProperty("uuid")]
	public string uuid { get; set; } = "";

	// game_id
	[Preserve]
	[JsonProperty("game_id")]
	public string game_id { get; set; } = "";

	// default constructor
	public RNFTWalletLoginRequest()
	{
		// Default constructor
	}	

	// constructor
	public RNFTWalletLoginRequest(string _uuid, string _game_id)
	{
		uuid = _uuid;
		game_id = _game_id;
	}
}

[Preserve]
[Serializable]
public class RNFTWalletLoginResponseData
{
	// uuid
	[Preserve]
	[JsonProperty("uuid")]
	public string uuid { get; set; } = "";

	// loggedIn
	[Preserve]
	[JsonProperty("loggedIn")]
	public bool loggedIn { get; set; } = false;

	// default constructor
	public RNFTWalletLoginResponseData()
	{
		// Default constructor
	}

	// constructor
	public RNFTWalletLoginResponseData(string _uuid, bool _loggedIn)
	{
		uuid = _uuid;
		loggedIn = _loggedIn;
	}
}

[Preserve]
[Serializable]
public class RNFTWalletLoginResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public RNFTWalletLoginResponseData data { get; set; } = new RNFTWalletLoginResponseData();

	// default constructor
	public RNFTWalletLoginResponse()
	{
		// Default constructor
	}

	// constructor
	public RNFTWalletLoginResponse(RNFTWalletLoginResponseData _data)
	{
		data = _data;
	}
}
