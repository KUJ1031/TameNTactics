using System.Diagnostics;
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
                return;

            hasTriggered = true;

            DialogueManager.Instance.StartDialogue(npcName, npcSprite, startID);

            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.isInputBlocked = true;
                playerController.BlockInput(true);
                DialogueManager.Instance.isCommunicationEneded = false;
            }
        }
    }

    private void Update()
    {
        if (hasTriggered && DialogueManager.Instance.isCommunicationEneded)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            var playerController = player.GetComponent<PlayerController>();
            if (player != null)
            {
                
                if (playerController != null)
                {
                    playerController.isInputBlocked = false;
                }
            }

            // 대화가 끝났을 때 중복 실행 가능하면 hasTriggered 초기화
            if (canRepeat)
            {
                playerController.BlockInput(true);
                hasTriggered = false;
            }

            // 만약 대화 종료 상태 플래그를 여기서 초기화한다면
            // DialogueManager.Instance.isCommunicationEneded = false;
        }
    }


}
