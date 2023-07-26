using System;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentityProvider;
using System.Collections.Generic;

public static class RNFTAuthHelpers
{
    public static string SignInUser(string email)
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
        string session = authResponse.Session;

        // log the response challenge type 
        Debug.Log("[RNFT] " + authResponse.ChallengeName + " has been initiated.");

        return session;
    }


    // function to verify the otp entered by the user
    public static RNFTAuthTokensType VerifyUserOTP(string email, string otp, string session)
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
        Amazon.CognitoIdentityProvider.Model.RespondToAuthChallengeResponse authChallengeResponseResult
            = providerClient.RespondToAuthChallengeAsync(authChallengeResponse).Result;


        // get the authentication result
        Amazon.CognitoIdentityProvider.Model.AuthenticationResultType authResult = authChallengeResponseResult.AuthenticationResult;

        // retreive the id tokem, the access token and the refresh token of the user
        string idToken = authResult.IdToken;
        string accessToken = authResult.AccessToken;
        string refreshToken = authResult.RefreshToken;


        // log the response challenge type
        Debug.Log("[RNFT] The custom challenge has been responded to.");

        // create the response
        RNFTAuthTokensType res = new RNFTAuthTokensType(idToken, accessToken, refreshToken);
        return res;
    }

    public static void SignUpUser(string email)
    {
        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // create a sign up user request 
        Amazon.CognitoIdentityProvider.Model.SignUpRequest signUpRequest = new Amazon.CognitoIdentityProvider.Model.SignUpRequest();

        // set the client id
        signUpRequest.ClientId = RNFTAuthConfig.UserPoolClientID;

        // set the username
        signUpRequest.Username = email;

        // set the password
        signUpRequest.Password = "password";

        // sign up the user and store the response
        Amazon.CognitoIdentityProvider.Model.SignUpResponse signUpResponse = providerClient.SignUpAsync(signUpRequest).Result;

        // log the response challenge type
        Debug.Log("[RNFT] The user has been signed up.");
    }

    public static RNFTUserDetails GetUserDetails(string accessToken)
    {
        // verify that the id token is not null
        if (accessToken == null)
        {
            Debug.Log("[RNFT] ID token is null, cannot get user details!");
            RNFTUserDetails _userDetails = new RNFTUserDetails();
            return _userDetails;
        }

        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // use the access token and the refresh token to get details such as the user's username
        Amazon.CognitoIdentityProvider.Model.GetUserRequest userRequest = new Amazon.CognitoIdentityProvider.Model.GetUserRequest();

        // set the access token
        userRequest.AccessToken = accessToken;

        // get the user details and store the response
        Amazon.CognitoIdentityProvider.Model.GetUserResponse userResponse = providerClient.GetUserAsync(userRequest).Result;

        // get the user's username
        string username = userResponse.Username;

        // get the user's email
        string email = userResponse.UserAttributes[2].Value;

        // return the user details object
        RNFTUserDetails userDetails = new RNFTUserDetails(username, email);
        return userDetails;
    }

    public static RNFTAuthTokensType RefreshAccessToken(string refreshToken)
    {
        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // create a refresh token request
        Amazon.CognitoIdentityProvider.Model.InitiateAuthRequest authRequest = new Amazon.CognitoIdentityProvider.Model.InitiateAuthRequest();

        // set the auth flow type to refresh token
        authRequest.AuthFlow = Amazon.CognitoIdentityProvider.AuthFlowType.REFRESH_TOKEN_AUTH;

        // set the client id
        authRequest.ClientId = RNFTAuthConfig.UserPoolClientID;

        // define the auth parameters
        Dictionary<string, string> authParams = new Dictionary<string, string>();
        authParams.Add("REFRESH_TOKEN", refreshToken);

        // set the auth parameters
        authRequest.AuthParameters = authParams;

        try
        {
            // initiate the auth request and store the response
            Amazon.CognitoIdentityProvider.Model.InitiateAuthResponse authResponse = providerClient.InitiateAuthAsync(authRequest).Result;

            // get the authentication result
            Amazon.CognitoIdentityProvider.Model.AuthenticationResultType authResult = authResponse.AuthenticationResult;

            // retreive the id tokem, the access token and the refresh token of the user
            string idToken = authResult.IdToken;
            string accessToken = authResult.AccessToken;
            string newRefreshToken = authResult.RefreshToken;

            // create a new auth tokens object
            RNFTAuthTokensType tokens = new RNFTAuthTokensType(idToken, accessToken, newRefreshToken);

            // return the tokens
            return tokens;
        }
        catch
        {
            Debug.Log("[RNFT] Refresh token is invalid!");
            RNFTAuthTokensType tokens = new RNFTAuthTokensType();
            return tokens;
        }
    }

    public static bool IsUserLoggedIn(string accessToken, string refreshToken)
    {

        // if the access token is null, the user is not logged in
        if (accessToken == null || accessToken == "")
        {
            return false;
        }

        if (refreshToken == null || refreshToken == "")
        {
            return false;
        }

        // check if the access token is still valid. if not, refersh the access token using the refresh token
        try
        {
            RNFTUserDetails userDetails = GetUserDetails(accessToken);
            return true;
        }
        catch (Exception e)
        {
            if (e.Message.Contains("NotAuthorizedException"))
            {
                // refresh the access token
                RNFTAuthTokensType tokens = RefreshAccessToken(refreshToken);
                string _accessToken = tokens.AccessToken;
                string _refreshToken = tokens.RefreshToken;
                string _idToken = tokens.IdToken;

                if (_accessToken == "" || _refreshToken == "" || _idToken == "")
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

