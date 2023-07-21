using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RNFTLogin : MonoBehaviour
{
    [SerializeField] private Button mainButton;
    [SerializeField] private GameObject loginContainer;

    // Start is called before the first frame update
    void Start()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(LoginUser);
    }

    private void LoginUser()
    {
        Debug.Log("onLoginClicked ");
        RNFTAuthManager _authenticationManager = new RNFTAuthManager();
        string loginUrl = _authenticationManager.GetLoginUrl();
        Application.OpenURL(loginUrl);
    }

}
