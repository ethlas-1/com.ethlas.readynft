using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class RNFTAuthManager : MonoBehaviour
{

    public static RNFTAuthManager Instance { get; private set; }

    public bool IsUserLoggedIn;


    // Use this for initialization
    async void Start()
	{
		await CheckUserAuth();		
	}

	// Update is called once per frame
	void Update()
	{
			
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

	async Task<bool> CheckUserAuth()
	{
        RNFTAuthTokensType tokens =  await RNFTAuthSessionHelpers.ReadAuthDataFileAsync();

        if (tokens.AccessToken == "" || tokens.RefreshToken == "")
        {
            return false;
        }

        bool isUserLoggedIn = RNFTAuthHelpers.IsUserLoggedIn(tokens);
        return isUserLoggedIn;
    }
}

