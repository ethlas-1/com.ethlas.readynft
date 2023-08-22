using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTDoesGhostWalletExistRequest
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
	public RNFTDoesGhostWalletExistRequest()
	{
		// Default constructor
	}

	// constructor
	public RNFTDoesGhostWalletExistRequest(string _euid, string _game_id)
	{
		euid = _euid;
		game_id = _game_id;
	}
}

[Preserve]
[Serializable]
public class RNFTDoesGhostWalletExistResponseData
{
	// exists 
	[Preserve]
	[JsonProperty("exists")]
	public bool exists { get; set; } = false;

	// default constructor
	public RNFTDoesGhostWalletExistResponseData()
	{
		// Default constructor
	}

	// constructor
	public RNFTDoesGhostWalletExistResponseData(bool _exists)
	{
		exists = _exists;
	}
}

[Preserve]
[Serializable]
public class RNFTDoesGhostWalletExistResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public RNFTDoesGhostWalletExistResponseData data { get; set; } = new RNFTDoesGhostWalletExistResponseData();

	// default constructor
	public RNFTDoesGhostWalletExistResponse()
	{
		// Default constructor
	}

	// constructor
	public RNFTDoesGhostWalletExistResponse(RNFTDoesGhostWalletExistResponseData _data)
	{
		data = _data;
	}
}
	


