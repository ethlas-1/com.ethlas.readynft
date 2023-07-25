using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RNFTUIManager : MonoBehaviour
{
    [SerializeField] private Button loginWithRNFTButton;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private GameObject loggedInWindow;

    void Start()
    {
        SetupRNFTButtonListeners();
    }

    // function to setup listeners for the ready nft button
    private void SetupRNFTButtonListeners()
    {
        loginWithRNFTButton.onClick.RemoveAllListeners();
        loginWithRNFTButton.onClick.AddListener(ShowLoginWindow);
    }

    // function to show the login window once it is clicked
    private void ShowLoginWindow()
    {
        uiContainer.SetActive(true);
        loginWindow.SetActive(true);
    }
    
}
