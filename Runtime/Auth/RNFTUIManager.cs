using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RNFTUIManager : MonoBehaviour
{
    public static RNFTUIManager Instance { get; private set; }

    [SerializeField] private Button loginWithRNFTButton;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private GameObject loggedInWindow;

    void Start()
    {
        SetupRNFTButtonListeners();
    }

    // function to setup listeners for the ready nft button
    public void SetupRNFTButtonListeners()
    {
        loginWithRNFTButton.onClick.RemoveAllListeners();
        loginWithRNFTButton.onClick.AddListener(ShowLoginWindow);
    }

    // function to show the login window once it is clicked
    public void ShowLoginWindow()
    {
        uiContainer.SetActive(true);
        loginWindow.SetActive(true);
    }

    // function to bring up login screen
    public void ShowUserProfile()
    {
        uiContainer.SetActive(true);
        loginWindow.SetActive(false);
        loggedInWindow.SetActive(true);
    }


    void Awake()
    {
        Debug.Log("[RNFT] UI Manager Awake!");


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

}
