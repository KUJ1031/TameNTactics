using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeBushes : MonoBehaviour
{

    public float eventCooldown = 1f;
    private bool isOnCooldown = false;
    private bool playerInZone = false;
    private GameObject player;

    public float respawnDelay = 5f;

    private Dictionary<string, float> itemDropChances = new();

    private void Start()
    {
        InitializeItemChances();
    }

    public void Investigate()
    {
        // 이 위치에서 재생성하도록 요청하지 않고, 단순히 파괴만 함
        UnknownForest.Instance.RequestBushRespawn(this.transform.position, respawnDelay);

        Destroy(gameObject);
    }

    private void InitializeItemChances()
    {
        itemDropChances["소형 회복 물약"] = 0.01f;
        itemDropChances["중형 회복 물약"] = 0.005f;
        itemDropChances["대형 회복 물약"] = 0.001f;
        itemDropChances["소형 전체 회복 물약"] = 0.007f;
        itemDropChances["중형 전체 회복 물약"] = 0.003f;
        itemDropChances["대형 전체 회복 물약"] = 0.001f;
        itemDropChances["이상한 물약"] = 0.0002f;
        itemDropChances["고기"] = 0.0001f;
    }

    private void Update()
    {
        if (playerInZone && !isOnCooldown && Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.Instance.StartDialogue("나", PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender], 7000);
        //OccurrenceNewEvent();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = true;
            player = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
            player = null;
        }
    }

    public void OccurrenceNewEvent()
    {
        float roll = Random.value;

        if (roll < 0.33f)
        {
            TryDropItem();
        }
        else if (roll < 0.66f)
        {
            Debug.Log("[미지의 숲] 몬스터가 나타났다! 전투 시작!");
            StartCoroutine(EventCooldown());
        }
        else
        {
            Debug.Log("[미지의 숲] 아무 일도 일어나지 않았다...");
            StartCoroutine(EventCooldown());
        }
        Investigate();
    }

    private void TryDropItem()
    {
        foreach (var itemData in ItemManager.Instance.consumableItems)
        {
            string itemName = itemData.itemName;

            if (itemDropChances.TryGetValue(itemName, out float chance))
            {
                if (Random.value < chance)
                {
                    Debug.Log($"[미지의 숲] '{itemName}' 아이템을 획득했습니다!");
                    PlayerManager.Instance.player.AddItem(itemName, 1);
                    StartCoroutine(EventCooldown());
                    return;
                }
            }
        }

        Debug.Log("[미지의 숲] 아이템 드롭 이벤트였지만 아무것도 얻지 못했습니다.");
        StartCoroutine(EventCooldown());
    }

    private IEnumerator EventCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(eventCooldown);
        isOnCooldown = false;
        Debug.Log("이벤트 쿨타임이 끝났습니다.");
    }
}
