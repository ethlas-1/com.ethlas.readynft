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

[Preserve]
[Serializable]
public class GetBalanceResponseData
{
	// totalBalance
	[Preserve]
	[JsonProperty("totalBalance")]
	public string totalBalance { get; set; } = "";

	// default constructor
	public GetBalanceResponseData()
	{
		// Default constructor
	}

	// constructor
	public GetBalanceResponseData(string _totalBalance)
	{
		totalBalance = _totalBalance;
	}
}


[Preserve]
[Serializable]
public class GetBalanceResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public GetBalanceResponseData data { get; set; } = new GetBalanceResponseData();

	// default constructor
	public GetBalanceResponse()
	{
		// Default constructor
	}

	// constructor
	public GetBalanceResponse(GetBalanceResponseData _data)
	{
		data = _data;
	}
}

[Preserve]
[Serializable]
public class LoginRequestData
{
	// provider
	[Preserve]
	[JsonProperty("provider")]
	public string provider { get; set; } = "";

	// default constructor
	public LoginRequestData()
	{
		// Default constructor
	}

	// constructor
	public LoginRequestData(string _provider)
	{
		provider = _provider;
	}
}


[Preserve]
[Serializable]
public class LoginResponseData
{
	// uid
	[Preserve]
	[JsonProperty("uid")]
	public string uid { get; set; } = "";

	// default constructor
	public LoginResponseData()
	{
		// Default constructor
	}

	// constructor
	public LoginResponseData(string _uid)
	{
		uid = _uid;
	}
}


[Preserve]
[Serializable]
public class LoginResponse
{
	// data
	[Preserve]
	[JsonProperty("data")]
	public LoginResponseData data { get; set; } = new LoginResponseData();

	// default constructor
	public LoginResponse()
	{
		// Default constructor
	}

	// constructor
	public LoginResponse(LoginResponseData _data)
	{
		data = _data;
	}
}
