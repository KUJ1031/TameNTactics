using UnityEngine;

public class RaycastEventTrigger : MonoBehaviour
{
    public enum RayDirection { Up, Down, Left, Right }

    [Header("NPC 설정")]
    public Sprite npcSprite;
    public string npcName = "이름 없음";
    public int startID;

    [Header("Ray 설정")]
    public RayDirection rayDirectionEnum = RayDirection.Right;
    public float rayDistance = 5f;
    public LayerMask playerLayer;
    public bool drawDebugRay = true;

    [Header("반복 설정")]
    public bool canRepeat = false;

    private bool hasTriggered = false;
    private bool playerInRay = false; // 플레이어가 레이 범위 내에 있는지
    private string uniqueID;

    private void Start()
    {
        uniqueID = gameObject.name + transform.position.ToString(); // 고유 ID 생성
        hasTriggered = DialogueSaveSystem.LoadRayTriggerState(uniqueID);
    }

    private void Update()
    {
        Vector2 direction = GetDirectionVector(rayDirectionEnum);

        // 박스 크기 정의
        Vector2 boxSize = (rayDirectionEnum == RayDirection.Left || rayDirectionEnum == RayDirection.Right)
            ? new Vector2(rayDistance, 0.2f)
            : new Vector2(0.2f, rayDistance);

        // 박스 중심을 Ray 방향으로 절반만큼 이동
        Vector2 boxCenter = (Vector2)transform.position + direction * (rayDistance / 2f);

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, playerLayer);


        if (hit != null && hit.CompareTag("Player"))
        {
            if (!playerInRay)
            {
                playerInRay = true;

                if (!hasTriggered || canRepeat)
                {
                    TriggerDialogue(hit.gameObject);
                }
            }
        }
        else
        {
            playerInRay = false;
        }
    }


    private Vector2 GetDirectionVector(RayDirection direction)
    {
        switch (direction)
        {
            case RayDirection.Up: return Vector2.up;
            case RayDirection.Down: return Vector2.down;
            case RayDirection.Left: return Vector2.left;
            case RayDirection.Right: return Vector2.right;
            default: return Vector2.right;
        }
    }

    private void TriggerDialogue(GameObject player)
    {
        hasTriggered = true;
        DialogueSaveSystem.SaveRayTriggerState(uniqueID, true);

        if (!DialogueManager.Instance.IsLoaded)
        {
            DialogueManager.Instance.OnDialogueLoaded += () => StartDialogue(player);
            return;
        }

        StartDialogue(player);
    }

    private void StartDialogue(GameObject player)
    {
        DialogueManager.Instance.StartDialogue(npcName, npcSprite, startID);

        var playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.isInputBlocked = true;
            DialogueManager.Instance.isCommunicationEneded = false;
        }

        DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnd;
    }

    private void HandleDialogueEnd()
    {
        DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnd;

        var playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.isInputBlocked = false;

        if (canRepeat)
        {
            hasTriggered = false;
            DialogueSaveSystem.SaveRayTriggerState(uniqueID, false);
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawDebugRay) return;

        Vector2 direction = GetDirectionVector(rayDirectionEnum);
        Vector2 boxSize = (rayDirectionEnum == RayDirection.Left || rayDirectionEnum == RayDirection.Right)
            ? new Vector2(rayDistance, 0.2f)
            : new Vector2(0.2f, rayDistance);

        Vector2 boxCenter = (Vector2)transform.position + direction * (rayDistance / 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
