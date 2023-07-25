using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentityProvider;

public class RNFTLoginWindowManager : MonoBehaviour
{

    public static RNFTLoginWindowManager Instance { get; private set; }

    [SerializeField] private InputField email;
    [SerializeField] private InputField otp;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button generateOTPButton;

    // submitted email and the session class vaiables
    public string submittedEmail;
    public string session; 

    void Start()
    {
        SetupLoginButton();
        SetupGenerateOTPButton();
    }

    void Awake()
    {
        Debug.Log("[RNFT] Passwordless Auth Manager Awake!");

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
            SignInUser(email.text);
        }
        else
        {
            Debug.Log("[RNFT] Email field is empty");
        }
    }

    // function to handle the login button click
    private void HandleLoginButtonClick()
    {
        // submitted email should not be "" or null
        if (otp.text != "" && submittedEmail != "" && submittedEmail != null)
        {
            VerifyUserOTP(submittedEmail, otp.text);
        }
        else
        {
            Debug.Log("OTP field is empty");
        }
    }


    // helper functions
    public void SignInUser(string email)
    {
        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // create a new initiate auth request
        Amazon.CognitoIdentityProvider.Model.InitiateAuthRequest authRequest = new Amazon.CognitoIdentityProvider.Model.InitiateAuthRequest();

        // set the auth flow type to custom auth
        authRequest.AuthFlow = Amazon.CognitoIdentityProvider.AuthFlowType.CUSTOM_AUTH;

        // set the client id
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

        // store the session and the email that was submitted in class variables
        session = authResponse.Session;
        submittedEmail = email;

        // log the response challenge type 
        Debug.Log("[RNFT] " + authResponse.ChallengeName + " has been initiated.");
    }


    // function to verify the otp entered by the user
    public void VerifyUserOTP(string email, string otp)
    {
        // create a response to the auth challenge
        Amazon.CognitoIdentityProvider.Model.RespondToAuthChallengeRequest authChallengeResponse = new Amazon.CognitoIdentityProvider.Model.RespondToAuthChallengeRequest();

        // set the client id
        authChallengeResponse.ClientId = RNFTAuthConfig.UserPoolClientID;

        // set the challenge name
        authChallengeResponse.ChallengeName = Amazon.CognitoIdentityProvider.ChallengeNameType.CUSTOM_CHALLENGE;

        // set the session
        authChallengeResponse.Session = session;

        // define the challenge responses
        Dictionary<string, string> challengeResponses = new Dictionary<string, string>();
        challengeResponses.Add("USERNAME", email);
        challengeResponses.Add("ANSWER", otp);

        // set the challenge responses
        authChallengeResponse.ChallengeResponses = challengeResponses;

        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // respond to the auth challenge and store the response
        Amazon.CognitoIdentityProvider.Model.RespondToAuthChallengeResponse authChallengeResponseResult = providerClient.RespondToAuthChallengeAsync(authChallengeResponse).Result;

        // log the response challenge type
        Debug.Log("[RNFT] The custom challenge has been responded to.");        

    }
}

