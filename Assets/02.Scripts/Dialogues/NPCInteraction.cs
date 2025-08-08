using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[System.Serializable]
public class SpeakerData
{
    public string speakerName;  // 화자 이름 (예: "NPC1", "NPC2")
    public Sprite speakerSprite; // 해당 화자의 이미지
}

public class NPCInteraction : MonoBehaviour
{
    public Sprite npcSprite;           // NPC 이미지
    public string npcName = "이름 없음"; // 말하는 사람 이름
    public int startID;                // 대화 시작 ID
    public List<SpeakerData> speakers = new();  // 여러 등장인물 이름/이미지 설정

    [Header("상호작용 키")]
    public string keySettingName = "Player.Interaction.0"; // 키 설정 이름

    private bool isPlayerTouching = false;
    private ButtonControl interactButton;
    private string lastKeyPath = "";

    [Header("UI")]
    public GameObject interactPromptObj; // UI 오브젝트 (ex: 텍스트가 담긴 오브젝트)
    public TMP_Text interactPromptText;  // 키 이름을 보여줄 TextMeshPro 텍스트
    private void Start()
    {
        // 키 경로 받아서 ButtonControl 캐싱
        if (PlayerManager.Instance.player.playerKeySetting.TryGetValue(keySettingName, out string path))
        {
            var control = InputSystem.FindControl(path);
            interactButton = control as ButtonControl;

            if (interactButton == null)
                Debug.LogWarning($"입력 경로 '{path}'에 해당하는 ButtonControl을 찾지 못했습니다.");
        }
        else
        {
            Debug.LogWarning($"키 설정 '{keySettingName}'이 없습니다.");
        }
        if (interactPromptObj != null)
            interactPromptObj.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = true;
           // playerController.isInputBlocked = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;

            if (interactPromptObj != null)
                interactPromptObj.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPlayerTouching)
        {
            if (interactPromptObj != null)
                interactPromptObj.SetActive(false);
            return;
        }

        UpdateInteractButton();

        // 프롬프트 표시
        if (interactPromptObj != null && interactButton != null)
        {
            interactPromptObj.SetActive(true);
            interactPromptText.text = $"[{interactButton.displayName}] 상호작용";
        }

        if (interactButton != null && interactButton.wasPressedThisFrame)
        {
            HandleInteraction();
        }

        HandleDialogueEnd();
    }

    private void UpdateInteractButton()
    {
        if (PlayerManager.Instance.player.playerKeySetting.TryGetValue(keySettingName, out string path))
        {
            if (path != lastKeyPath)
            {
                lastKeyPath = path;
                interactButton = InputSystem.FindControl(path) as ButtonControl;
            }
        }
    }

    private void HandleInteraction()
    {
        Interact();

        if (PlayerManager.Instance.playerController != null)
        {
            PlayerManager.Instance.playerController.isInputBlocked = true;
            PlayerManager.Instance.playerController.lastMoveInput = Vector2.zero;
        }
    }

    private void HandleDialogueEnd()
    {
        if (DialogueManager.Instance.isCommunicationEneded)
        {
            PlayerManager.Instance.playerController.isInputBlocked = false;
            //Debug.Log("대화 종료: 입력 차단 해제됨");
        }
    }

    public void Interact()
    {
        if (speakers != null && speakers.Count > 0)
        {
            // 멀티 캐릭터 방식 (speakers 리스트에 따라 처리)
            string mainSpeaker = speakers[0].speakerName;
            Sprite mainSprite = speakers[0].speakerSprite;

            DialogueManager.Instance.StartDialogue(
                mainSpeaker, // 트리 ID (CSV ID도 이 이름과 일치)
                mainSprite,
                startID
            );
        }
        else
        {
            // 기존 단일 캐릭터 방식
            DialogueManager.Instance.StartDialogue(
                npcName,
                npcSprite,
                startID
            );
        }
    }

}
