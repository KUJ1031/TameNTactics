using UnityEngine;
using UnityEngine.UI;

public class CloseFieldUIButton : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(OnClickCloseMenuButton);
    }
    private void OnClickCloseMenuButton()
    {
        FieldUIManager.Instance.CloseAllUI();
    }
}
