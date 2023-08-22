using System.Collections.Generic;
using System;
using UnityEngine.Scripting;
using UnityEngine;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTEuidTransferRequest
{
	// from
	[Preserve]
	[JsonProperty("from")]
	public string from { get; set; } = "";

	// to
	[Preserve]
	[JsonProperty("to")]
	public string to { get; set; } = "";

	// default constructor
	public RNFTEuidTransferRequest()
	{
		// Default constructor
	}	

	// constructor
	public RNFTEuidTransferRequest(string _from, string _to)
	{
		from = _from;
		to = _to;
	}
}