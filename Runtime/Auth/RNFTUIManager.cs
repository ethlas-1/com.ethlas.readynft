using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RNFTUIManager : MonoBehaviour
{
    public static RNFTUIManager Instance { get; private set; }

    [SerializeField] private Button loginWithRNFTButton;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private GameObject loggedInWindow;
    [SerializeField] private Button bgBtn;
    [SerializeField] private Button closeButton;

    public System.Action OnShowRNFTLogin;
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

        // bgBtn.onClick.RemoveAllListeners();
        // bgBtn.onClick.AddListener(HideReadyNFTScreen);

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
        uiContainer.SetActive(false);
        loginWindow.SetActive(false);
        loggedInWindow.SetActive(false);
    }

    // function to bring up logged in  screen
    public void ShowUserProfile()
    {
        uiContainer.SetActive(true);
        loginWindow.SetActive(false);
        loggedInWindow.SetActive(true);

        OnShowRNFTLoggedIn?.Invoke();
    }

    // function to bring up log in screen
    public void ShowLoginScreen()
    {
        uiContainer.SetActive(true);
        loggedInWindow.SetActive(false);
        loginWindow.SetActive(true);

        OnShowRNFTLogin?.Invoke();
    }

}
