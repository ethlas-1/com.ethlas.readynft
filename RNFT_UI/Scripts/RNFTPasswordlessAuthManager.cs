using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentityProvider;

public class RNFTPasswordlessAuthManager
{
    public RNFTPasswordlessAuthManager()
    {
        // default constructor
    }

    public void SignIn(string email)
    {
        // create a new instance of the cognito identity provider client
        AmazonCognitoIdentityProviderClient providerClient = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APSouthEast1);

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

        Debug.Log("response is ", authResponse);


    }
}
