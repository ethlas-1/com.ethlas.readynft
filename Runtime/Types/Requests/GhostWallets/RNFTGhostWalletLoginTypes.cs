using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTGhostWalletLoginRequest
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
	public RNFTGhostWalletLoginRequest()
	{
		// Default constructor
	}	

	// constructor
	public RNFTGhostWalletLoginRequest(string _euid, string _game_id)
	{
		euid = _euid;
		game_id = _game_id;
	}
}

[Preserve]
[Serializable]
public class RNFTGhostWalletLoginResponseData
{
	// euid
	[Preserve]
	[JsonProperty("euid")]
	public string euid { get; set; } = "";

	// loggedIn
	[Preserve]
	[JsonProperty("loggedIn")]
	public bool loggedIn { get; set; } = false;

	// default constructor
	public RNFTGhostWalletLoginResponseData()
	{
		// Default constructor
	}

	// constructor
	public RNFTGhostWalletLoginResponseData(string _euid, bool _loggedIn)
	{
		euid = _euid;
		loggedIn = _loggedIn;
	}
}

[Preserve]
[Serializable]
public class RNFTGhostWalletLoginResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public RNFTGhostWalletLoginResponseData data { get; set; } = new RNFTGhostWalletLoginResponseData();

	// default constructor
	public RNFTGhostWalletLoginResponse()
	{
		// Default constructor
	}

	// constructor
	public RNFTGhostWalletLoginResponse(RNFTGhostWalletLoginResponseData _data)
	{
		data = _data;
	}
}

