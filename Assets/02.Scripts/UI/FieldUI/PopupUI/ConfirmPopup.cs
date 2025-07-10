using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Button okButton, noButton;

    private System.Action<bool> onResult;

    private void Awake()
    {
        okButton.onClick.AddListener(OnClickOK);
        noButton.onClick.AddListener(OnClickCancel);
    }

    public void Open(string message, System.Action<bool> callback)
    {
        popupText.text = message;
        onResult = callback;
        gameObject.SetActive(true);
    }

    //ok버튼
    private void OnClickOK()
    {
        onResult?.Invoke(true);
        Destroy(gameObject);
    }
    //no버튼
    private void OnClickCancel()
    {
        onResult?.Invoke(false);
        Destroy(gameObject);
    }
}

