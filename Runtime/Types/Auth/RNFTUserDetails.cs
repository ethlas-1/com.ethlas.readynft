using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Newtonsoft.Json;


[Preserve]
[Serializable]
public class RNFTUserDetails
{
	[Preserve]
    [JsonProperty("UID")]
	public string UID { get; set; } = "";

	[Preserve]
    [JsonProperty("email")]
	public string email { get; set; } = "";

	[Preserve]
	[JsonProperty("custodialWalletAddress")]
	public string custodialWalletAddress { get; set; } = "";

	public RNFTUserDetails()
	{
		// default constructor
	}

	public RNFTUserDetails(string uid, string email, string custodialWalletAddress)
	{
		this.UID = uid;
		this.email = email;
		this.custodialWalletAddress = custodialWalletAddress;
	}

	public RNFTUserDetails(string uid, string email)
	{
		this.UID = uid;
		this.email = email;
	}
}

