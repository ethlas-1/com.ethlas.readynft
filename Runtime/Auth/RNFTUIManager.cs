using UnityEngine;
using UnityEngine.UI;

public class RNFTUIManager : MonoBehaviour
{
    public static RNFTUIManager Instance { get; private set; }

    [SerializeField] private Button loginWithRNFTButton;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private GameObject otpWindow;
    [SerializeField] private GameObject loggedInWindow;
    [SerializeField] private Button closeButton;

    public System.Action OnShowRNFTLogin;
    public System.Action OnHideRNFTScreen;
    public System.Action OnShowRNFTLoggedIn;

    private void Awake()
    {
        Debug.Log("[RNFT] UI Manager Awake!");


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

         SetupRNFTButtonListeners();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // function to setup listeners for the ready nft button
    public void SetupRNFTButtonListeners()
    {
        loginWithRNFTButton.onClick.RemoveAllListeners();
        loginWithRNFTButton.onClick.AddListener(ShowReadyNFTScreen);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(HideReadyNFTScreen);
    }

    // function to show the login window once it is clicked
    public void ShowReadyNFTScreen()
    {

        bool? isLoggedIn = RNFTAuthManager.Instance?.IsUserLoggedIn;

        if (isLoggedIn == false || isLoggedIn == null)
        {
            ShowLoginScreen();
        } else
        {
            ShowUserProfile();
        }
    }

    // function to hide uiContainer
    public void HideReadyNFTScreen()
    {
        RNFTLogger.LogEvent("RNFT_ui_close_button");
        uiContainer.SetActive(false);
        loginWindow.SetActive(false);
        otpWindow.SetActive(false);
        loggedInWindow.SetActive(false);

        OnHideRNFTScreen?.Invoke();
    }

    // function to bring up logged in  screen
    public void ShowUserProfile()
    {
        RNFTLogger.LogEvent("RNFT_ui_user_profile");
        uiContainer.SetActive(true);
        loginWindow.SetActive(false);
        otpWindow.SetActive(false);
        loggedInWindow.SetActive(true);

        OnShowRNFTLoggedIn?.Invoke();
    }

    // function to bring up log in screen
    public void ShowLoginScreen()
    {
        RNFTLogger.LogEvent("RNFT_ui_login");
        uiContainer.SetActive(true);
        loggedInWindow.SetActive(false);
        loginWindow.SetActive(true);
        otpWindow.SetActive(false);

        OnShowRNFTLogin?.Invoke();
    }

}
