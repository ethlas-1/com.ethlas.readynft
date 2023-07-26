using System;

[Serializable]
public class RNFTUserDetails
{
	public string UID { get; set; } = "";
	public string email { get; set; } = "";
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

