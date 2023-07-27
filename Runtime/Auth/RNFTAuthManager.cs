using System.Security.AccessControl;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class RNFTAuthManager : MonoBehaviour
{

    public static RNFTAuthManager Instance { get; private set; }
    public System.Action<bool> OnUserLoginCallback;

    public bool IsUserLoggedIn;
    public RNFTAuthTokensType tokens;
    public RNFTUserDetails userDetials;

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
        RNFTAuthTokensType tokens =  await RNFTAuthSessionHelpers.ReadAuthDataFileAsync();

        if (tokens.AccessToken == "" || tokens.RefreshToken == "")
        {
            return;
        }

        bool _isUserLoggedIn = RNFTAuthHelpers.IsUserLoggedIn(tokens);

        if (_isUserLoggedIn)
        {
            Debug.Log("[RNFT] Logged in user detected");
            this.tokens = tokens;
            RNFTUserDetails _userDetials = RNFTAuthHelpers.GetUserDetails(tokens.AccessToken);

            RNFTUserDetails userDetailsFromDB = await RNFTAuthHelpers.FetchUserDetailsFromDB(_userDetials.UID, _userDetials.email);
            this.userDetials = userDetailsFromDB;
        }

        this.IsUserLoggedIn = _isUserLoggedIn;

        // callback for the login status
        OnUserLoginCallback?.Invoke(this.IsUserLoggedIn);
    }

    void LogOutUser()
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
    public void SetUserLoggedInStatus(bool status)
    {
        this.IsUserLoggedIn = status;
        // callback for the login status
        OnUserLoginCallback?.Invoke(this.IsUserLoggedIn);
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
}

