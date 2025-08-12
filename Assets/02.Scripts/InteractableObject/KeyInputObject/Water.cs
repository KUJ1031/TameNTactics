using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Water : KeyInputObject
{

    [Header("물 오브젝트 설정")]
    public TextMeshPro interactionHintText; // 상호작용 힌트 UI 텍스트

    protected override void Interact()
    {
        Debug.Log("Water : (상호작용 키 : " + inputControlPath + ")");
        // 열림 애니메이션, 상태 변경 등
    }

    protected override void ShowInteractionHint(bool show)
    {
        if (show)
        {
            Debug.Log("물과 상호작용할 수 있습니다. (" + inputControlPath + ")");
            if (interactionHintText != null)
            {
                string readableKey = InputControlPath.ToHumanReadableString(
                    inputControlPath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice |
                    InputControlPath.HumanReadableStringOptions.UseShortNames
                );

                interactionHintText.text = readableKey;
                interactionHintText.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("물과의 상호작용이 종료되었습니다.");
            if (interactionHintText != null)
            {
                interactionHintText.gameObject.SetActive(false);
            }
        }
    }

}
