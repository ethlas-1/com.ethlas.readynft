using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class RNFTAuthManager : MonoBehaviour
{

    public static RNFTAuthManager Instance { get; private set; }
    public System.Action<bool> OnUserLoginCallback;

    public string apiKey;
    public string gameId;

    public bool IsUserLoggedIn = false;
    public RNFTAuthTokensType tokens;
    public RNFTUserDetails userDetials;
    private string RNFTGhostWalletAddress = "";
    private string ExternalUid = "";
    private bool RNFTAuthCheckDone = false;

    // Use this for initialization
    void Start()
    {
        CheckUserAuth();
    }

    void Awake()
    {
        Debug.Log("[RNFT] Auth Manager Awake!");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void CheckUserAuth()
    {
        RNFTAuthTokensType tokens = await RNFTAuthSessionHelpers.ReadAuthDataFileAsync();

        if (tokens.AccessToken == "" || tokens.RefreshToken == "")
        {
            return;
        }

        var (_isUserLoggedIn, newTokens) = RNFTAuthHelpers.IsUserLoggedIn(tokens);
        SetTokens(newTokens);
        RNFTAuthSessionHelpers.UpdateAuthSessionData(newTokens);

        if (_isUserLoggedIn)
        {
            Debug.Log("[RNFT] Logged in user detected");
            RNFTUserDetails _userDetials = RNFTAuthHelpers.GetUserDetails(newTokens.AccessToken);

            RNFTUserDetails userDetailsFromDB = await RNFTAuthHelpers.FetchUserDetailsFromDB(_userDetials.UID, _userDetials.email);
            this.userDetials = userDetailsFromDB;
        }

        this.IsUserLoggedIn = _isUserLoggedIn;
        Debug.Log("RNFT Auth Check done has been set to true already");
        this.RNFTAuthCheckDone = true;

        // callback for the login status
        OnUserLoginCallback?.Invoke(this.IsUserLoggedIn);

    }

    public void LogOutUser()
    {
        RNFTAuthHelpers.RevokeAccessToken(this.tokens);
        RNFTAuthSessionHelpers.DeleteAuthDataFile();

        SetUserLoggedInStatus(false);
        this.tokens = new RNFTAuthTokensType();
        this.userDetials = new RNFTUserDetails();

        RNFTUIManager.Instance?.ShowLoginScreen();

        Debug.Log("[RNFT] User logged out.");
    }

    // method to set the user logged in status
    public async void SetUserLoggedInStatus(bool status)
    {
        this.IsUserLoggedIn = status;
        // callback for the login status
        OnUserLoginCallback?.Invoke(this.IsUserLoggedIn);

        if (!status) return;

        // user logs into rnft
        // if the user has a ready nft wallet, then sign in on chain with the ready nft wallet
        // and transfer the assets from the ghost wallet to the ready nft wallet 
        string _custodialWalletAddress = this.userDetials.custodialWalletAddress;
        bool isRNFTWalletPresent = _custodialWalletAddress != "" && _custodialWalletAddress != null;
        string uuid = this.userDetials.UID;

        if (this.RNFTGhostWalletAddress != "" && isRNFTWalletPresent)
        {
            // transfer the assets from the ghost wallet to the ready nft wallet (dont await)
            RNFTGhostWalletHelpers.BindAccount(this.ExternalUid, uuid, "uuid");

            // sign in on chain with the ready nft wallet (dont await)
            RNFTGhostWalletHelpers.WalletLogin(uuid, this.gameId);
        }
        else
        {
            // transfer ownership of the ghost wallet to the rnft user (await)
            await RNFTGhostWalletHelpers.TrfGhostWalletToRNFTUser(this.ExternalUid, uuid);
        }
    }

    // method to set the tokens
    public void SetTokens(RNFTAuthTokensType tokens)
    {
        this.tokens = tokens;
    }

    // method to set the user details
    public void SetUserDetails(RNFTUserDetails userDetails)
    {
        this.userDetials = userDetails;
    }

    public void SetRNFTAuthCheckDone(bool _done)
    {
        if (!this.RNFTAuthCheckDone && !string.IsNullOrEmpty(this.ExternalUid) && _done)
        {
            string uuid = this.userDetials.UID;
            EUIDInitHandler(this.ExternalUid, uuid);
        }

        this.RNFTAuthCheckDone = _done;
    }

    // method to set the external uid
    public async void SetExternalUid(string externalUid)
    {
        Debug.Log("[RNFT] Setting external uid with uid " + externalUid);
        if (externalUid == "")
        {
            Debug.Log("[RNFT] External UID is empty");
            return;
        }

        // if ExternalUid is not null or empty, then there is a change in euid
        string uuid = this.userDetials.UID;
        bool isEUIDChanging = this.ExternalUid != "" && this.ExternalUid != null;
        if (isEUIDChanging)
        {
            Debug.Log("[RNFT]: The EUID Is changing");
            await EUIDChangeHandler(externalUid);

        }
        else
        {
            Debug.Log("[RNFT]: The EUID Is being set for the first time");
            if (this.RNFTAuthCheckDone) await EUIDInitHandler(externalUid, uuid);
        }

        this.ExternalUid = externalUid;
    }

    public async Task<bool> EUIDChangeHandler(string externalUid)
    {
        Debug.Log("[RNFT]: The EUID is changing");
        // does ghost wallet exist for external uid (the new one)
        bool doesGhostWalletExistForNewEUID = await RNFTGhostWalletHelpers.DoesGhostWalletExist(externalUid, this.gameId);

        if (doesGhostWalletExistForNewEUID)
        {
            Debug.Log("[RNFT]: The New EUID HAS an existing Ghost Wallet");
            RNFTGhostWalletHelpers.BindAccount(this.ExternalUid, externalUid, "euid");
            RNFTGhostWalletHelpers.GhostWalletLogin(externalUid, this.gameId);
        }
        else
        {
            Debug.Log("[RNFT]: The New EUID does not have an existing Ghost Wallet");
            await RNFTGhostWalletHelpers.EuidTransfer(this.ExternalUid, externalUid);
        }
        return true;
    }

    public async Task<bool> EUIDInitHandler(string externalUid, string uuid)
    {
        Debug.Log("[RNFT]: EUID Init Handler Trigerred");
        if (this.IsUserLoggedIn)
        {
            Debug.Log("[RNFT]: User has already logged into RNFT");
            // user details have already been fethced and strored in the user details 
            // trigger the on chain rnft wallet sign in method
            RNFTGhostWalletHelpers.WalletLogin(uuid, this.gameId);
        }
        else
        {
            Debug.Log("[RNFT]: User has not logged into RNFT");
            // use the euid to fetch the ghost wallet and store it 
            string _ghostWallet = await RNFTGhostWalletHelpers.FetchGhostWallet(externalUid, this.gameId);
            SetRNFTGhostWalletAddress(_ghostWallet);

            // trigger the ghost wallet on chain sing in method
            RNFTGhostWalletHelpers.GhostWalletLogin(externalUid, this.gameId);
        }

        return true;
    }

    // method to set the RNFTGhostWalletAddress
    public void SetRNFTGhostWalletAddress(string RNFTGhostWalletAddress)
    {
        if (RNFTGhostWalletAddress == "")
        {
            Debug.Log("[RNFT] RNFTGhostWalletAddress is empty");
            return;
        }
        Debug.Log("[RNFT]: SetRNFTGhostWalletAddress with Ghost Wallet Address " + RNFTGhostWalletAddress);

        this.RNFTGhostWalletAddress = RNFTGhostWalletAddress;
    }
}

