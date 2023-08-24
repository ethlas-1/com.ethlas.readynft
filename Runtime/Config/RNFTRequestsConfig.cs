using System;

public static class RNFTRequestsConfig
{
	// API endpoints root url
	public const string API_ENDPOINTS_ROOT_URL = "https://aws.ethlas.com/prod";

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

    // API ghost wallet exists route
    public const string API_GHOST_WALLET_EXISTS_ROUTE = "/doesGhostWalletExist";

    // API fetch ghost wallet route
    public const string API_FETCH_GHOST_WALLET_ROUTE = "/fetchGhostWallet";

    // API trf ghost wallet to rnft user route
    public const string API_TRF_GHOST_WALLET_TO_RNFT_USER_ROUTE = "/trfGhostWalletToRNFTUser";

    // API ghost wallet login route
    public const string API_GHOST_WALLET_LOGIN_ROUTE = "/ghostWalletLogin";

    // API ghost wallet exists route
    public const string API_BIND_ACCOUNT_ROUTE = "/readyNFT/bindAccount";

    // API wallet login  route
    public const string API_WALLET_LOGIN_ROUTE = "/walletLogin";

    // API for euid account transfer
    public const string API_EUID_TRANSFER_ROUTE = "/readyNFT/euidTransfer";

}
