using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTTrfGWToRNFTUserRequest
{
	// euid 
	[Preserve]
	[JsonProperty("euid")]
	public string euid { get; set; } = "";

	// uuid
	[Preserve]
	[JsonProperty("uuid")]
	public string uuid { get; set; } = "";

	// default constructor
	public RNFTTrfGWToRNFTUserRequest()
	{
		// Default constructor
	}

	// constructor
	public RNFTTrfGWToRNFTUserRequest(string _euid, string _uuid)
	{
		euid = _euid;
		uuid = _uuid;
	}
}

[Preserve]
[Serializable]
public class RNFTTrfGWToRNFTUserResponseData
{
	// transferred
	[Preserve]
	[JsonProperty("transferred")]
	public bool transferred { get; set; } = false;

	// default constructor
	public RNFTTrfGWToRNFTUserResponseData()
	{
		// Default constructor
	}

	// constructor
	public RNFTTrfGWToRNFTUserResponseData(bool _transferred)
	{
		transferred = _transferred;
	}
}

[Preserve]
[Serializable]
public class RNFTTrfGWToRNFTUserResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public RNFTTrfGWToRNFTUserResponseData data { get; set; } = new RNFTTrfGWToRNFTUserResponseData();

	// default constructor
	public RNFTTrfGWToRNFTUserResponse()
	{
		// Default constructor
	}

	// constructor
	public RNFTTrfGWToRNFTUserResponse(RNFTTrfGWToRNFTUserResponseData _data)
	{
		data = _data;
	}
}

