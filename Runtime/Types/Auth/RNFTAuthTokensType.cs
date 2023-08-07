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

	[Preserve]
	[JsonProperty("Session")]
	public string Session { get; set; } = ""; // the session that is used to log the user in

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

	public RNFTAuthTokensType(string idToken, string accessToken, string refreshToken, string session)
	{
		this.IdToken = idToken;
		this.AccessToken = accessToken;
		this.RefreshToken = refreshToken;
		this.Session = session;
	}
}

