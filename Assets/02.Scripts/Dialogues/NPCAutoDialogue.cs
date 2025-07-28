using System.Diagnostics;
using TMPro;
using UnityEngine;

public class NPCAutoDialogue : MonoBehaviour
{
    public Sprite npcSprite;           // NPC 이미지
    public string npcName = "이름 없음"; // 말하는 사람 이름
    public int startID;                // 대화 시작 ID

    private bool hasTriggered = false; // 중복 실행 방지

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            if (!DialogueManager.Instance.IsLoaded)
            {
                return;
            }

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

    private void Update()
    {
        // 대화 종료되면 다시 이동 가능
        if (hasTriggered && DialogueManager.Instance.isCommunicationEneded)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.isInputBlocked = false;
                }
            }

           // DialogueManager.Instance.isCommunicationEneded = false;
        }
    }

}
