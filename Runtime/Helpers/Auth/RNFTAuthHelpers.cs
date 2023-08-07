using System;
using UnityEngine;
using UnityEngine.Networking;
using Amazon;
using Amazon.CognitoIdentityProvider;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;


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
        try
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

            // check if the authentication result attribute exists before trying to access it
            if (authChallengeResponseResult.AuthenticationResult == null)
            {
                Debug.Log("[RNFT] The authentication result is null.");
                string newSession = authChallengeResponseResult.Session;
                RNFTAuthTokensType failedAttemptRes = new RNFTAuthTokensType();
                failedAttemptRes.Session = newSession;
                return failedAttemptRes;
            }

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
        catch (Exception e)
        {
            Debug.Log("[RNFT] OTP verification Error: " + e.ToString());
            RNFTAuthTokensType res = new RNFTAuthTokensType();
            return res;
        }
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

    public static RNFTAuthTokensType RefreshAccessToken(RNFTAuthTokensType currTokens)
    {
        string refreshToken = currTokens.RefreshToken;

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
            string accessToken = authResult.AccessToken;

            // create a new auth tokens object
            RNFTAuthTokensType tokens = new RNFTAuthTokensType(currTokens.IdToken, accessToken, currTokens.RefreshToken);

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

    public static (bool, RNFTAuthTokensType) IsUserLoggedIn(RNFTAuthTokensType tokens)
    {
        string accessToken = tokens.AccessToken;
        string refreshToken = tokens.RefreshToken;

        // if the access token is null, the user is not logged in
        if (accessToken == null || accessToken == "")
        {
            return (false, new RNFTAuthTokensType());
        }

        if (refreshToken == null || refreshToken == "")
        {
            return (false, new RNFTAuthTokensType());
        }

        // check if the access token is still valid. if not, refersh the access token using the refresh token
        try
        {
            RNFTUserDetails userDetails = GetUserDetails(accessToken);
            return (true, tokens);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("NotAuthorizedException") || e.Message.Contains("Access Token has expired"))
            {
                // refresh the access token
                RNFTAuthTokensType newTokens = RefreshAccessToken(tokens);
                string _accessToken = newTokens.AccessToken;
                string _refreshToken = newTokens.RefreshToken;
                string _idToken = newTokens.IdToken;
                return (true, newTokens);
            }
            else
            {
                return (false, new RNFTAuthTokensType());
            }
        }
    }

    // function to revoke the access token of the user
    public static void RevokeAccessToken(RNFTAuthTokensType tokens)
    {

        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSoutheast1);

        // create a revoke token request
        Amazon.CognitoIdentityProvider.Model.GlobalSignOutRequest globalSignOutRequest = new Amazon.CognitoIdentityProvider.Model.GlobalSignOutRequest();

        // set the access token
        globalSignOutRequest.AccessToken = tokens.AccessToken;

        try
        {
            // revoke the access token
            providerClient.GlobalSignOutAsync(globalSignOutRequest);
            Debug.Log("[RNFT] The access token has been revoked.");
        }
        catch (Exception e)
        {
            Debug.Log("[RNFT] There was an error while trying to revoke the access token.");
            Debug.Log(e.Message);
        }
    }

    // function to fetch the user data from the database
    public static async Task<RNFTUserDetails> FetchUserDetailsFromDB(string uid, string email)
    {

        if (string.IsNullOrEmpty(uid))
        {
            Debug.Log("FetchUserDetailsFromDB: UID not set!");
            return new RNFTUserDetails();
        }

        string url = RNFTRequestsConfig.API_ENDPOINTS_ROOT_URL + RNFTRequestsConfig.API_FETCH_USER_DETAILS_FROM_DB_ROUTE;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                FetchUserDataFromDBRequestData requestData = new FetchUserDataFromDBRequestData(uid, email);
                string requestDataJson = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    FetchUserDataFromDBResponse result = JsonConvert.DeserializeObject<FetchUserDataFromDBResponse>(responseString);

                    string _uid = result.data.uid;
                    string _email = result.data.email;
                    string _custodialWalletAddress = result.data.custodialWalletAddress;

                    RNFTUserDetails userDetails = new RNFTUserDetails(_uid, _email, _custodialWalletAddress);
                    return userDetails;
                }
                else
                {
                    Debug.Log("FetchUserDetailsFromDB: Request failed: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("FetchUserDetailsFromDB: Request failed: " + e.Message);
            }
        }

        return new RNFTUserDetails(); // return empty list if request fails
    }
}

