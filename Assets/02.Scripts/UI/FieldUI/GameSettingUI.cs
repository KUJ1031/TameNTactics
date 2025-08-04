using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingUI : FieldMenuBaseUI
{
    [SerializeField] private Button audioSettingButton, keySettingButton, closeMenuButton;
    [SerializeField] private GameObject audioSettingUI;
    [SerializeField] private GameObject keySettingUI;


    private void Awake()
    {
        audioSettingButton.onClick.AddListener(OnClickAudioSettingButton);
        keySettingButton.onClick.AddListener(OnClickKeySettingButton);
        if (SceneManager.GetActiveScene().name == "MainMapScene")
        {
            closeMenuButton.onClick.AddListener(OnClickCloseMenuButton);
        }
    }

    private void OnClickAudioSettingButton()
    {
        ToggleSettingView(true);
    }
    private void OnClickKeySettingButton()
    {
        ToggleSettingView(false);
    }
    private void OnClickCloseMenuButton()
    {
        FieldUIManager.Instance.CloseAllUI();
    }

    private void ToggleSettingView(bool isAudio)
    {
        audioSettingUI.SetActive(isAudio);
        keySettingUI.SetActive(!isAudio);
    }
}
