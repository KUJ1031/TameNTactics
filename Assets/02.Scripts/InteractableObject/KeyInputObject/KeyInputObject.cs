using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Collider2D))]
public class KeyInputObject : MonoBehaviour
{
    [Header("상호작용 키")]
    public string keySettingName = "Player.Interaction.0"; // 설정 키 이름

    [Header("상호작용 범위 감지")]
    public string playerTag = "Player";
    private bool isPlayerInRange = false;

    protected string inputControlPath;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;

            // 최신 키값을 반영
            if (PlayerManager.Instance.player.playerKeySetting.TryGetValue(keySettingName, out string path))
            {
                inputControlPath = path;
            }

            ShowInteractionHint(true); // 이 시점엔 최신 키값으로 UI 갱신
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            ShowInteractionHint(false); // 힌트 UI 숨기기
        }
    }

    private void Update()
    {
        if (!isPlayerInRange) return;

        if (PlayerManager.Instance.player.playerKeySetting.TryGetValue(keySettingName, out string path))
        {
            inputControlPath = path;
            var control = InputSystem.FindControl(path);

            if (control is ButtonControl button && button.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }


    // 상호작용 로직 정의
    protected virtual void Interact()
    {
        Debug.Log($"{gameObject.name} 상호작용");
        // 여기에 오브젝트별 기능을 오버라이드해서 구현
    }

    // 힌트 표시/숨김 처리용 (옵션)
    protected virtual void ShowInteractionHint(bool show)
    {
        // 힌트 UI 표시/비활성화 처리
        // 예: interactionHintUI.SetActive(show);
    }
}
