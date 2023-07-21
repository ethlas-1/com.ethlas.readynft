using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RNFTLogin : MonoBehaviour
{
    [SerializeField] private Button mainButton;
    [SerializeField] private GameObject loginContainer;
    private RNFTAuthManager _authManager;

    // Start is called before the first frame update
    void Start()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(LoginUser);
    }

    private void LoginUser()
    {
        Debug.Log("[RNFT] Ready NFT Login Has Started.");

        // fetching the auth manager from the hierarchy
        _authManager = FindObjectOfType<RNFTAuthManager>();

        // check to ensure that the auth manager is not null
        if (_authManager == null)
        {
            Debug.Log("[RNFT] Auth manager is null, cannot process deep link!");
            return;
        }

        string loginUrl = _authManager.GetLoginUrl();
        Application.OpenURL(loginUrl);
    }

}
