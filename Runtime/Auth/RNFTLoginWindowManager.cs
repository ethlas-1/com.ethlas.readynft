using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


}

