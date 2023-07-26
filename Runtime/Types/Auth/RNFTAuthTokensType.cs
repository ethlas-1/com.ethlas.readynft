using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Newtonsoft.Json;

[Preserve]
[Serializable]
public class RNFTAuthTokensType
{
    [Preserve]
    [JsonProperty("IdToken")]
    public string IdToken { get; set; } = "";

    [Preserve]
    [JsonProperty("AccessToken")]
    public string AccessToken { get; set; } = "";

    [Preserve]
    [JsonProperty("RefreshToken")]
    public string RefreshToken { get; set; } = "";

	public RNFTAuthTokensType()
	{
		// default constructor
	}

	public RNFTAuthTokensType(string idToken, string accessToken, string refreshToken)
	{
		this.IdToken = idToken;
		this.AccessToken = accessToken;
		this.RefreshToken = refreshToken;
	}	
}

