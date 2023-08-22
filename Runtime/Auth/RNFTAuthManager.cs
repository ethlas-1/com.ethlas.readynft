﻿using System.Security.AccessControl;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class RNFTAuthManager : MonoBehaviour
{

    public static RNFTAuthManager Instance { get; private set; }
    public System.Action<bool> OnUserLoginCallback;

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
    public void SetUserLoggedInStatus(bool status)
    {
        this.IsUserLoggedIn = status;
        // callback for the login status
        OnUserLoginCallback?.Invoke(this.IsUserLoggedIn);

        // user logs into rnft
        // if the user has a ready nft wallet, then sign in on chain with the ready nft wallet
        // and transfer the assets from the ghost wallet to the ready nft wallet 
        string _custodialWalletAddress = this.userDetials.custodialWalletAddress;
        bool isRNFTWalletPresent = _custodialWalletAddress != "" && _custodialWalletAddress != null;
        if (this.RNFTGhostWalletAddress != "" && isRNFTWalletPresent)
        {
            // TODO: transfer the assets from the ghost wallet to the ready nft wallet (dont await)
            // TODO: sign in on chain with the ready nft wallet (dont await)
        }
        else
        {
            // TODO: transfer ownership of the ghost wallet to the rnft user (await)
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

    // method to set the external uid
    public void SetExternalUid(string externalUid)
    {
        if (externalUid == "")
        {
            Debug.Log("[RNFT] External UID is empty");
            return;
        }

        // if ExternalUid is not null or empty, then there is a change in euid
        bool isEUIDChanging = this.ExternalUid != "" && this.ExternalUid != null;
        if (isEUIDChanging)
        {
            // TODO: does ghost wallet exist for external uid (the new one)
            bool doesGhostWalletExistForNewEUID = false;

            if (doesGhostWalletExistForNewEUID)
            {
                // TODO: transfer the assets from the ghost wallet to the ready nft wallet (dont await)
                // TODO: sign in on chain with the ready nft wallet (dont await)
            } else {
                // TODO: trnasfer ownership of the ghost wallet from euid 1 to euid 2 (await)
            }

        }
        else
        {
            if (this.RNFTAuthCheckDone && this.IsUserLoggedIn)
            {
                // user details have already been fethced and strored in the user details 
                // trigger the on chain rnft wallet sign in method
                // TODO: implement the on chain rnft wallet sign in method
            }
            else
            {
                // use the euid to fetch the ghost wallet 
                // TODO: call the fetch ghost wallet method here
                // store the ghost wallet in the ghost wallet address field
                // trigger the ghost wallet on chain sing in method
                // TODO: implement the ghost wallet on chain sing in method
            }

        }



        this.ExternalUid = externalUid;
    }

    // method to set the RNFTGhostWalletAddress
    public void SetRNFTGhostWalletAddress(string RNFTGhostWalletAddress)
    {
        if (RNFTGhostWalletAddress == "")
        {
            Debug.Log("[RNFT] RNFTGhostWalletAddress is empty");
            return;
        }

        this.RNFTGhostWalletAddress = RNFTGhostWalletAddress;
    }
}

