using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private string idToken;
    private string accessToken;
    private string refreshToken;

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
        string _submittedEmail = email.text;
        if (_submittedEmail != "")
        {
            string _session = RNFTAuthHelpers.SignInUser(_submittedEmail);
            session = _session;
            submittedEmail = _submittedEmail;
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
            RNFTAuthTokensType user = RNFTAuthHelpers.VerifyUserOTP(submittedEmail, otp.text, session);
            Debug.Log("the user is " + user.UID);
        }
        else
        {
            Debug.Log("One of the required fields is empty");
        }
    }
}


