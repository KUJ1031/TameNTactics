using Unity.VisualScripting;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Sprite npcSprite;           // NPC 이미지
    public string npcName = "이름 없음"; // 말하는 사람 이름
    public int startID;        // 이 NPC의 대화 시작 ID (기본값 100)

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(
            npcName,   // 대화 트리 ID
            npcSprite, // NPC 이미지
            startID    // 시작 노드 ID
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"NPC {npcName}와 상호작용 시작");
            Interact();
        }
    }
}
