using UnityEngine;
using UnityEngine.UI;

public class LearnMorePopupManager : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject learnMorePopup;

    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => HandleCloseButton());
    }

    private void HandleCloseButton ()
    {
        learnMorePopup.SetActive(false);
        RNFTLogger.LogEvent("RNFT_login_window_learnmore_close");
    }
}
