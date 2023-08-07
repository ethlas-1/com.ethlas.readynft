using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OTPInputManager : MonoBehaviour
{
    public System.Action<bool> OnUserLoginCallback;

    [SerializeField] private InputField otp1;
    [SerializeField] private InputField emailField;
    [SerializeField] private Text helpText;
    [SerializeField] private Button backButton;

    private bool isLoggingIn = false;


    // Start is called before the first frame update
    void Start()
    {
        otp1.onValueChanged.AddListener(onValueChangeHandler);

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => HandleBackButtonPress());
    }

    void OnEnable()
    {
        otp1.Select();
        RNFTLogger.LogEvent("RNFT_otp_window_show");
    }

    void OnDisable()
    {
        ResetOTPInput();
    }

    private void HandleBackButtonPress()
    {
        RNFTUIManager.Instance?.ShowLoginScreen();
        helpText.text = "";
        helpText.gameObject.SetActive(false);
        ResetOTPInput();
        RNFTLogger.LogEvent("RNFT_otp_window_backbutton");
    }

    private void ResetOTPInput()
    {
        otp1.Select();
        otp1.text = "";
    }

    private void onValueChangeHandler(string input)
    {
        if (input == "")
        {
            return;
        }

        if (input.Length == 6)
        {
            //  auto login once 6 digits are provided            
            HandleLoginButtonClick(RNFTLoginWindowManager.Instance.submittedEmail, input, RNFTLoginWindowManager.Instance.session);
        }

    }

    // function to handle the login button click
    private async void HandleLoginButtonClick(string submittedEmail, string otp, string session)
    {
        RNFTLogger.LogEvent("RNFT_otp_window_otp_input_finish");
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

                if (tokens.Session != null && tokens.Session != "")
                {
                    RNFTLoginWindowManager.Instance?.SetSession(tokens.Session);
                }

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
            ResetOTPInput();

            isLoggingIn = false;
        }
        else
        {
            Debug.Log("One of the required fields is empty");
        }
        RNFTLogger.LogEvent("RNFT_otp_window_otp_success");
        isLoggingIn = false;
    }
}