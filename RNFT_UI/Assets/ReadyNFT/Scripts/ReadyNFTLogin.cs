using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyNFTLogin : MonoBehaviour
{
    [SerializeField] private Button mainButton;
    [SerializeField] private GameObject loginContainer;

    // Start is called before the first frame update
    void Start()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(ShowMainWindow);
    }

    private void ShowMainWindow()
    {
        loginContainer.SetActive(true);
    }

}
