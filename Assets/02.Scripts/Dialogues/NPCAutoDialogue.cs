using TMPro;
using UnityEngine;

public class NPCAutoDialogue : MonoBehaviour
{
    public Sprite npcSprite;           // NPC 이미지
    public string npcName = "이름 없음"; // 말하는 사람 이름
    public int startID;                // 대화 시작 ID

    [SerializeField] private bool canRepeat = false; // 대화 중복 실행 여부
    private bool hasTriggered = false; // 중복 실행 방지

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((!hasTriggered || canRepeat) && other.CompareTag("Player"))
        {
            if (!DialogueManager.Instance.IsLoaded)
            {
                // 아직 로드 안 됐으면, 로드 완료 후 다시 시도
                DialogueManager.Instance.OnDialogueLoaded += () => TryStartDialogue(other);
                return;
            }

            TryStartDialogue(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((!hasTriggered || canRepeat) && other.CompareTag("Player"))
        {
            if (!DialogueManager.Instance.IsLoaded)
                return;

            hasTriggered = true;

            DialogueManager.Instance.StartDialogue(npcName, npcSprite, startID);

            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.isInputBlocked = true;
                DialogueManager.Instance.isCommunicationEneded = false;
            }
        }
    }
    private void TryStartDialogue(Collider2D other)
    {
        if (hasTriggered && !canRepeat) return;

        hasTriggered = true;

        // 이벤트 구독
        DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnd;

        DialogueManager.Instance.StartDialogue(npcName, npcSprite, startID);

        PlayerManager.Instance.playerController.isInputBlocked = true;
        DialogueManager.Instance.isCommunicationEneded = false;
    }

    private void HandleDialogueEnd()
    {
        var playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        if (playerController != null && playerController.isInputBlocked)
        {
            playerController.isInputBlocked = false;
            Debug.Log("Dialogue ended, restoring player input.");
        }

        // 이벤트 해제
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnd;

        if (canRepeat)
        {
            hasTriggered = false;
        }
    }


}
