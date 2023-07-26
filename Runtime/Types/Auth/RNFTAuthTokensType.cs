using System;

[Serializable]
public class RNFTAuthTokensType
{
	public string IdToken { get; set; } = "";
	public string AccessToken { get; set; } = "";
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

