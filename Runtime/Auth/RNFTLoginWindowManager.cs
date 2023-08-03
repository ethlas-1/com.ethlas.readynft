﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class RNFTLoginWindowManager : MonoBehaviour
{
    public static RNFTLoginWindowManager Instance { get; private set; }

    public System.Action<bool> OnUserLoginCallback;


    [SerializeField] private InputField email;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button generateOTPButton;
    [SerializeField] private Text helpText;
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private GameObject otpWindow;

    // submitted email and the session class vaiables
    public string submittedEmail;
    public string session;

    private bool isOTPRunning = false;
    private float timeMax = 30f;
    private float timeCounter = 0f;

    void Awake()
    {
        Debug.Log("[RNFT] RNFTLoginWindowManager Awake!");

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

    void Start()
    {
        SetupGenerateOTPButton();
    }

    void OnEnable()
    {
        isOTPRunning = false;
        helpText.gameObject.SetActive(false);
        generateOTPButton.interactable = true;
    }

    void OnDisable()
    {
    }

    private void Update()
    {
        if (!isOTPRunning)
            return;

        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0f)
        {
            isOTPRunning = false;
            generateOTPButton.interactable = true;
        }
        else
        {
        }
    }

    // function to setup listeners for the generate otp button
    private void SetupGenerateOTPButton()
    {
        generateOTPButton.onClick.RemoveAllListeners();
        generateOTPButton.onClick.AddListener(HandleGenerateOTPButtonClick);
    }

    // function to hanlde the generate otp button click 
    private void HandleGenerateOTPButtonClick()
    {
        helpText.gameObject.SetActive(false);

        // convert the email to all lower case
        string _submittedEmail = email.text.ToLower();
        if (_submittedEmail == "")
        {
            Debug.Log("[RNFT] Email field is empty");
            return;
        }

        if(!IsValidEmail(_submittedEmail))
        {
            helpText.text = "Please make sure provided email is correct.";
            helpText.gameObject.SetActive(true);

            return;
        }

        string _session;
        // check if the error thrown is due to the cognito user not existing using a try catch

        try
        {            
            _session = RNFTAuthHelpers.SignInUser(_submittedEmail);

            isOTPRunning = true;
            timeCounter = timeMax;
            generateOTPButton.interactable = false;
            loginWindow.SetActive(false);
            otpWindow.SetActive(true);
            helpText.text = "Check your email for the OTP.";
            helpText.gameObject.SetActive(true);
        }
        catch (Exception e)
        {
            // check if error is due to user not existing

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

    bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}


