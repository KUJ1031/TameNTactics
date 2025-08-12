using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MinimapToggle : MonoBehaviour
{
    [Header("미니맵 패널")]
    public string keySettingName = "Player.Map.0";
    public GameObject minimapPanel; // RawImage 포함한 전체 오브젝트

    private void Update()
    {
        if (PlayerManager.Instance.player.playerKeySetting.TryGetValue(keySettingName, out string path))
            {
            var control = InputSystem.FindControl(path);

            if (control is ButtonControl button && button.wasPressedThisFrame)
            {
                if (minimapPanel != null)
                {
                    bool isActive = minimapPanel.activeSelf;
                    minimapPanel.SetActive(!isActive);
                }
            }
            
        }
    }
}
