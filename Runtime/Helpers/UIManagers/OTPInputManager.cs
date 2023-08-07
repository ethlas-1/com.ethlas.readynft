using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        otp1.onValueChanged.AddListener(onValueChangeHandler1);
        otp2.onValueChanged.AddListener(onValueChangeHandler2);
        otp3.onValueChanged.AddListener(onValueChangeHandler3);
        otp4.onValueChanged.AddListener(onValueChangeHandler4);
        otp5.onValueChanged.AddListener(onValueChangeHandler5);
        otp6.onValueChanged.AddListener(onValueChangeHandler6);

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => HandleBackButtonPress());
    }

    void OnEnable()
    {
        ResetOTPInput();
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

    private void handleNextField(InputField current, InputField next, string input)
    {
        if (input.Length == 2)
        {

            current.interactable = false;
            current.text = input.Substring(0, 1);
            IEnumerator myWaitCoroutine()
            {
                next.interactable = true;
                next.Select();
                yield return new WaitForSeconds(0.05f);
                next.text = input.Substring(1);
            }
            StartCoroutine(myWaitCoroutine());
        }
    }

    private bool handleBackSpace(InputField current, InputField previous, string input)
    {
        if (input == "")
        {
            current.interactable = false;
            previous.interactable = true;
            previous.Select();
            return true;
        }
        return false;
    }

    private void onValueChangeHandler1(string input)
    {

        //  code to handle pasting on first input
        if(Time.time - time < 0.01 && input.Length <= 5)
        {
            time = Time.time;
            return;
        }
        time = Time.time;
        pasteOTP(input);

        if (input == "")
        {
            return;
        }

        handleNextField(otp1, otp2, input);
    }

    private void onValueChangeHandler2(string input)
    {
        bool backspace = handleBackSpace(otp2, otp1, input);
        if (backspace) return;
        
        handleNextField(otp2, otp3, input);
    }

    private void onValueChangeHandler3(string input)
    {
        bool backspace = handleBackSpace(otp3, otp2, input);
        if (backspace) return;

        handleNextField(otp3, otp4, input);
    }

    private void onValueChangeHandler4(string input)
    {
        bool backspace = handleBackSpace(otp4, otp3, input);
        if (backspace) return;

        handleNextField(otp4, otp5, input);
    }

    private void onValueChangeHandler5(string input)
    {
        bool backspace = handleBackSpace(otp5, otp4, input);
        if (backspace) return;

        handleNextField(otp5, otp6, input);
    }

    private void onValueChangeHandler6(string input)
    {
        bool backspace = handleBackSpace(otp6, otp5, input);
        if (backspace) return;

        string finalOTP = otp1.text + otp2.text + otp3.text + otp4.text + otp5.text + otp6.text;
        //  auto login once 6 digits are provided
        HandleLoginButtonClick(RNFTLoginWindowManager.Instance.submittedEmail, finalOTP, RNFTLoginWindowManager.Instance.session);
    }

    private void pasteOTP (string input)
    {
        if(input.Length == 6)
        {
            otp1.text = input.Substring(0, 1);
            otp2.text = input.Substring(1, 1);
            otp3.text = input.Substring(2, 1);
            otp4.text = input.Substring(3, 1);
            otp5.text = input.Substring(4, 1);
            otp6.text = input.Substring(5, 1);

            otp1.interactable = false;
            otp2.interactable = false;
            otp3.interactable = false;
            otp4.interactable = false;
            otp5.interactable = false;
            otp6.interactable = true;
            otp6.Select();
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