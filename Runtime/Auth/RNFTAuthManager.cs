using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class RNFTAuthManager : MonoBehaviour
{

    public static RNFTAuthManager Instance { get; private set; }

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

        this.IsUserLoggedIn = RNFTAuthHelpers.IsUserLoggedIn(tokens);
        this.tokens = tokens;
        this.userDetials = RNFTAuthHelpers.GetUserDetails(tokens.AccessToken);
    }

    // method to set the user logged in status
    public void SetUserLoggedInStatus(bool status)
    {
        this.IsUserLoggedIn = status;
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

