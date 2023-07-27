using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class RNFTLoginWindowManager : MonoBehaviour
{
    public System.Action<bool> OnUserLoginCallback;


    [SerializeField] private InputField email;
    [SerializeField] private InputField otp;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button generateOTPButton;

    // submitted email and the session class vaiables
    public string submittedEmail;
    public string session;
    private string idToken;
    private string accessToken;
    private string refreshToken;

    void Start()
    {
        SetupLoginButton();
        SetupGenerateOTPButton();
    }
    
    // function to setup listeners for the generate otp button
    private void SetupGenerateOTPButton()
    {
        generateOTPButton.onClick.RemoveAllListeners();
        generateOTPButton.onClick.AddListener(HandleGenerateOTPButtonClick);
    }

    // function to setup listeners for the login button
    private void SetupLoginButton()
    {
        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(HandleLoginButtonClick);
    }

    // function to hanlde the generate otp button click 
    private void HandleGenerateOTPButtonClick()
    {
        // convert the email to all lower case
        string _submittedEmail = email.text.ToLower();
        if (_submittedEmail == "")
        {
            Debug.Log("[RNFT] Email field is empty");
            return;
        }

        string _session;
        // check if the error thrown is due to the cognito user not existing using a try catch

        try
        {
            _session = RNFTAuthHelpers.SignInUser(_submittedEmail);

        }
        catch (Exception e)
        {
            // check if eroor is due to user not existing

            if (!e.Message.Contains("User does not exist"))
            {
                Debug.Log("[RNFT] " + e.Message);
                return;
            }

            RNFTAuthHelpers.SignUpUser(_submittedEmail);
            HandleGenerateOTPButtonClick();
            return;
        }

        session = _session;
        submittedEmail = _submittedEmail;


    }

    // function to handle the login button click
    private async void HandleLoginButtonClick()
    {
        // submitted email should not be "" or null
        if (otp.text != "" && submittedEmail != "" && submittedEmail != null)
        {
            RNFTAuthTokensType tokens = RNFTAuthHelpers.VerifyUserOTP(submittedEmail, otp.text, session);
            string accessToken = tokens.AccessToken;

            if (accessToken == "" || accessToken == null)
            {
                OnUserLoginCallback?.Invoke(false);
                return;

            }

            // get the user details
            RNFTUserDetails userDetails = RNFTAuthHelpers.GetUserDetails(accessToken);
            string uid = userDetails.UID;

            // ensure that uid is not an empty string or null
            if (uid == "" || uid == null)
            {
                OnUserLoginCallback?.Invoke(false);

                return;
            }

            OnUserLoginCallback?.Invoke(true);
            RNFTAuthSessionHelpers.UpdateAuthSessionData(tokens);
            RNFTUserDetails userDetailsFromDB = await RNFTAuthHelpers.FetchUserDetailsFromDB(uid, submittedEmail);
            RNFTAuthManager.Instance?.SetUserDetails(userDetailsFromDB);
            RNFTAuthManager.Instance?.SetTokens(tokens);
            RNFTAuthManager.Instance?.SetUserLoggedInStatus(true);
            RNFTUIManager.Instance?.ShowUserProfile();

        }
        else
        {
            Debug.Log("One of the required fields is empty");
        }
    }
}


