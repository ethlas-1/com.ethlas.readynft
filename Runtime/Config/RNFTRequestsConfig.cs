using System;

public static class RNFTRequestsConfig
{
	// API endpoints root url
	public const string API_ENDPOINTS_ROOT_URL = "https://3caea960hb.execute-api.ap-southeast-1.amazonaws.com/prod";

	// API ready nft route
    public const string API_READY_NFT_ROUTE = "/readyNFT";

    // API fetch sprites route
    public const string API_FETCH_SPRITES_ROUTE = "/v1/fetchSprites";

    // API fetch owned nfts route
    public const string API_FETCH_OWNED_NFTS_ROUTE = "/v1/fetchOnChainNFTsFromEmail";

    // API fetch user details from db
    public const string API_FETCH_USER_DETAILS_FROM_DB_ROUTE = "/fetchUserDetails";

    // API mint nft route
    public const string API_MINT_NFT_ROUTE = "/mintNFT";

}
