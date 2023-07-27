using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RNFTLoggedInWindowManager : MonoBehaviour
{
    [SerializeField] private Button copyBtn;
    [SerializeField] private Button logoutBtn;
    [SerializeField] private Button walletBtn;
    [SerializeField] private Button portalBtn;
    [SerializeField] private Text emailText;
    [SerializeField] private Text walletText;

    private void OnEnable() 
    {
        SetupPanel();
        SetupButtons();
    }

    private void SetupPanel()
    {
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
        if (RNFTAuthManager.Instance.IsUserLoggedIn)
        {
            GUIUtility.systemCopyBuffer = RNFTAuthManager.Instance.userDetials.custodialWalletAddress;
        }
    }

    private void HandleLogoutButtonClick()
    {
        if (RNFTAuthManager.Instance.IsUserLoggedIn)
        {
            RNFTAuthManager.Instance.LogOutUser();
        }
    }
}


