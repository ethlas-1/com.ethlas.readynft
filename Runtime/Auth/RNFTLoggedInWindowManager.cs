using UnityEngine;
using UnityEngine.UI;
using System;

public class RNFTLoggedInWindowManager : MonoBehaviour
{
    [SerializeField] private Button copyBtn;
    [SerializeField] private Button logoutBtn;
    [SerializeField] private Text emailText;
    [SerializeField] private Text walletText;
    
    private void OnEnable() 
    {
        SetupPanel();
        SetupButtons();
        RNFTLogger.LogEvent("RNFT_loggedin_window_show");
    }

    private void SetupPanel()
    {
        if (!RNFTAuthManager.Instance)
        {
            Debug.Log("auth manager is not instantiated");
            return;
        };

        if (RNFTAuthManager.Instance.IsUserLoggedIn)
        {
            emailText.text = RNFTAuthManager.Instance.userDetials.email;
            string walletAdd = RNFTAuthManager.Instance.userDetials.custodialWalletAddress;
            string first6Chara = 
            !String.IsNullOrEmpty(walletAdd) && walletAdd.Length >= 6
            ? walletAdd.Substring(0, 6)
            : walletAdd;

            string last4Chara = 
            !String.IsNullOrEmpty(walletAdd) && walletAdd.Length >= 4
            ? walletAdd.Substring(walletAdd.Length - 4) 
            : walletAdd;
            
            walletText.text = first6Chara + "..." + last4Chara;
        }
    }

    private void SetupButtons()
    {
        copyBtn.onClick.RemoveAllListeners();
        copyBtn.onClick.AddListener(() => HandleCopyButtonClick());

        logoutBtn.onClick.RemoveAllListeners();
        logoutBtn.onClick.AddListener(() => HandleLogoutButtonClick());
    }

    private void HandleCopyButtonClick()
    {
        RNFTLogger.LogEvent("RNFT_loggedin_window_copy_button");

        if (RNFTAuthManager.Instance.IsUserLoggedIn)
        {
            GUIUtility.systemCopyBuffer = RNFTAuthManager.Instance.userDetials.custodialWalletAddress;
        }
    }

    private void HandleLogoutButtonClick()
    {
        RNFTLogger.LogEvent("RNFT_loggedin_logout_button");

        if (RNFTAuthManager.Instance.IsUserLoggedIn)
        {
            RNFTAuthManager.Instance.LogOutUser();
        }
    }

    private void HandlePortalButtonClick()
    {
        Application.OpenURL("https://ethlas.com/readynft/");
    }

    private void HandleWalletButtonClick()
    {
        if (RNFTAuthManager.Instance.IsUserLoggedIn)
        {
            Application.OpenURL($"https://bscscan.com/address/{RNFTAuthManager.Instance.userDetials.custodialWalletAddress}");
        }
    }
}


