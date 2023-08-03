using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class OTPInputManager : MonoBehaviour
{
    public System.Action<bool> OnUserLoginCallback;

    [SerializeField] private InputField otp1;
    [SerializeField] private InputField otp2;
    [SerializeField] private InputField otp3;
    [SerializeField] private InputField otp4;
    [SerializeField] private InputField otp5;
    [SerializeField] private InputField otp6;
    [SerializeField] private InputField emailField;
    [SerializeField] private Text helpText;
    [SerializeField] private Button backButton;

    private bool isLoggingIn = false;


    // Start is called before the first frame update
    void Start()
    {
        otp1.Select();

        otp1.onValueChanged.AddListener(onValueChangeHanlder1);
        otp2.onValueChanged.AddListener(onValueChangeHanlder2);
        otp3.onValueChanged.AddListener(onValueChangeHanlder3);
        otp4.onValueChanged.AddListener(onValueChangeHanlder4);
        otp5.onValueChanged.AddListener(onValueChangeHanlder5);
        otp6.onValueChanged.AddListener(onValueChangeHanlder6);

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => HandleBackButtonPress());
    }

    private void HandleBackButtonPress()
    {
        RNFTUIManager.Instance?.ShowLoginScreen();
        helpText.text = "";
        helpText.gameObject.SetActive(false);
        ResetOTPInput();
    }

    private void ResetOTPInput()
    {
        otp1.Select();
        otp1.text = "";
        otp2.text = "";
        otp3.text = "";
        otp4.text = "";
        otp5.text = "";
        otp6.text = "";

        otp1.interactable = true;
        otp2.interactable = false;
        otp3.interactable = false;
        otp4.interactable = false;
        otp5.interactable = false;
        otp6.interactable = false;
    }

    private void onValueChangeHanlder1 (string input)
    {
        if(input == "")
        {
            return;
        }
        int value = Int32.Parse(input);
        if(value >= 0 && value <= 9)
        {
            otp1.interactable = false;
            otp2.interactable = true;
            otp2.Select();
        }
    }

    private void onValueChangeHanlder2 (string input)
    {
        if(input == "")
        {
            otp2.interactable = false;
            otp1.interactable = true;
            otp1.Select();
            return;
        }
        int value = Int32.Parse(input);
        if(value >= 0 && value <= 9)
        {
            otp2.interactable = false;
            otp3.interactable = true;
            otp3.Select();
        }
    }

    private void onValueChangeHanlder3 (string input)
    {
        if(input == "")
        {
            otp3.interactable = false;
            otp2.interactable = true;
            otp2.Select();
            return;
        }
        int value = Int32.Parse(input);
        if(value >= 0 && value <= 9)
        {
            otp3.interactable = false;
            otp4.interactable = true;
            otp4.Select();
        }
    }

    private void onValueChangeHanlder4 (string input)
    {
        if(input == "")
        {
            otp4.interactable = false;
            otp3.interactable = true;
            otp3.Select();
            return;
        }
        int value = Int32.Parse(input);
        if(value >= 0 && value <= 9)
        {
            otp4.interactable = false;
            otp5.interactable = true;
            otp5.Select();
        }
    }

    private void onValueChangeHanlder5 (string input)
    {
        if(input == "")
        {
            otp5.interactable = false;
            otp4.interactable = true;
            otp4.Select();
            return;
        }
        int value = Int32.Parse(input);
        if(value >= 0 && value <= 9)
        {
            otp5.interactable = false;
            otp6.interactable = true;
            otp6.Select();
        }
    }

    private void onValueChangeHanlder6 (string input)
    {
        if(input == "")
        {
            otp6.interactable = false;
            otp5.interactable = true;
            otp5.Select();
            return;
        }
        int value = Int32.Parse(input);
        if(value >= 0 && value <= 9)
        {
            //  otp6.interactable = false;
            //  todo    - auto submit 6 OTP code!
            string finalOTP = otp1.text + otp2.text + otp3.text + otp4.text + otp5.text + otp6.text;

            //  auto login once 6 digits are provided
            HandleLoginButtonClick(RNFTLoginWindowManager.Instance.submittedEmail, finalOTP, RNFTLoginWindowManager.Instance.session);
        }
    }

    // function to handle the login button click
    private async void HandleLoginButtonClick(string submittedEmail, string otp, string session)
    {
        if (isLoggingIn)
            return;

        helpText.gameObject.SetActive(false);

        // submitted email should not be "" or null
        if (submittedEmail != "" && submittedEmail != null)
        {
            isLoggingIn = true;
            
            RNFTAuthTokensType tokens = RNFTAuthHelpers.VerifyUserOTP(submittedEmail, otp, session);
            string accessToken = tokens.AccessToken;

            if (accessToken == "" || accessToken == null)
            {
                helpText.text = "Invalid OTP. Please try again.";
                helpText.gameObject.SetActive(true);
                OnUserLoginCallback?.Invoke(false);
                isLoggingIn = false;
                return;
            }

            // get the user details
            RNFTUserDetails userDetails = RNFTAuthHelpers.GetUserDetails(accessToken);
            string uid = userDetails.UID;

            // ensure that uid is not an empty string or null
            if (uid == "" || uid == null)
            {
                OnUserLoginCallback?.Invoke(false);
                isLoggingIn = false;
                return;
            }

            OnUserLoginCallback?.Invoke(true);
            RNFTAuthSessionHelpers.UpdateAuthSessionData(tokens);
            RNFTUserDetails userDetailsFromDB = await RNFTAuthHelpers.FetchUserDetailsFromDB(uid, submittedEmail);
            RNFTAuthManager.Instance?.SetUserDetails(userDetailsFromDB);
            RNFTAuthManager.Instance?.SetTokens(tokens);
            RNFTAuthManager.Instance?.SetUserLoggedInStatus(true);
            RNFTUIManager.Instance?.ShowUserProfile();

            emailField.text = "";

            isLoggingIn = false;
        }
        else
        {
            Debug.Log("One of the required fields is empty");
        }

        isLoggingIn = false;
    }
}
