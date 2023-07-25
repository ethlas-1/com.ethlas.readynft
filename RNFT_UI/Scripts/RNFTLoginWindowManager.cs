using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentityProvider;

public class RNFTLoginWindowManager : MonoBehaviour
{
    [SerializeField] private InputField email;
    [SerializeField] private InputField otp;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button generateOTPButton;

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
        if (email.text != "")
        {
            Debug.Log("email is " + email.text);
            SignIn(email.text);
        }
        else
        {
            Debug.Log("Email field is empty");
        }
    }

    // function to handle the login button click
    private void HandleLoginButtonClick()
    {
        // both otp and email are required to login
        if (otp.text != "" && email.text != "")
        {
            Debug.Log("otp is " + otp.text + " and email is " + email.text);
        }
        else
        {
            Debug.Log("OTP field is empty");
        }
    }

    public void SignIn(string email)
    {
        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // create a new initiate auth request
        Amazon.CognitoIdentityProvider.Model.InitiateAuthRequest authRequest = new Amazon.CognitoIdentityProvider.Model.InitiateAuthRequest();

        // set the auth flow type to custom auth
        authRequest.AuthFlow = Amazon.CognitoIdentityProvider.AuthFlowType.CUSTOM_AUTH;

        // set the client id
        Debug.Log("the client id is " + RNFTAuthConfig.UserPoolClientID);
        authRequest.ClientId = RNFTAuthConfig.UserPoolClientID;

        // define the auth parameters
        Dictionary<string, string> authParams = new Dictionary<string, string>();
        authParams.Add("USERNAME", email);
        authParams.Add("PASSWORD", "password");
        // password is not used for verfication. hence, it doesn't matter
        // we use an otp to verify the user instead, which is sent to their email

        // set the auth parameters
        authRequest.AuthParameters = authParams;

        // initiate the auth request and store the response
        Amazon.CognitoIdentityProvider.Model.InitiateAuthResponse authResponse = providerClient.InitiateAuthAsync(authRequest).Result;

        // log the response challenge type 
        Debug.Log("response is " + authResponse.ChallengeName);


    }



}

