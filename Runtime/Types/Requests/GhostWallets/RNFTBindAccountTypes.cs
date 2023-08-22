using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTBindAccountRequest
{
	// from
	[Preserve]
	[JsonProperty("from")]
	public string from { get; set; } = "";

	// to
	[Preserve]
	[JsonProperty("to")]
	public string to { get; set; } = "";

	// toType
	[Preserve]
	[JsonProperty("toType")]
	public string toType { get; set; } = "";

	// default constructor
	public RNFTBindAccountRequest()
	{
		// Default constructor
	}	

	// constructor
	public RNFTBindAccountRequest(string _from, string _to, string _toType)
	{
		from = _from;
		to = _to;
		toType = _toType;
	}
}

[Preserve]
[Serializable]
public class RNFTBindAccountResponseData
{
	// from
	[Preserve]
	[JsonProperty("from")]
	public string from { get; set; } = "";

	// loggedIn
	[Preserve]
	[JsonProperty("loggedIn")]
	public bool loggedIn { get; set; } = false;

	// default constructor
	public RNFTBindAccountResponseData()
	{
		// Default constructor
	}

	// constructor
	public RNFTBindAccountResponseData(string _from, bool _loggedIn)
	{
		from = _from;
		loggedIn = _loggedIn;
	}
}