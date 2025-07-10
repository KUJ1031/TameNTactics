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

    public void Open(PopupType type, string message, System.Action<bool> callback)
    {
        noButton.gameObject.SetActive(type == PopupType.Confirm); // 확인/경고에 따라 표시
        popupText.text = message;
        onResult = callback;
        gameObject.SetActive(true);
    }

    private void OnClickOK()
    {
        onResult?.Invoke(true);
        PopupUIManager.Instance.ClosePanel(gameObject);
    }

    private void OnClickCancel()
    {
        onResult?.Invoke(false);
        PopupUIManager.Instance.ClosePanel(gameObject);
    }
}
