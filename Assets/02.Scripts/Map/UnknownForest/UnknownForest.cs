using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownForest : MonoBehaviour
{
    public int startDelay = 3;
    public float eventCooldown = 1f;

    private bool isOnCooldown = false;

    // 아이템 드롭 확률 딕셔너리 (아이템 이름 기준)
    private Dictionary<string, float> itemDropChances = new();

    private void Start()
    {
        InitializeItemChances();
    }

    private void InitializeItemChances()
    {
        // 확률 설정
        itemDropChances["소형 회복 물약"] = 0.01f;
        itemDropChances["중형 회복 물약"] = 0.005f;
        itemDropChances["대형 회복 물약"] = 0.001f;

        itemDropChances["소형 전체 회복 물약"] = 0.007f;
        itemDropChances["중형 전체 회복 물약"] = 0.003f;
        itemDropChances["대형 전체 회복 물약"] = 0.001f;

        itemDropChances["이상한 물약"] = 0.0002f;
        itemDropChances["고기"] = 0.0001f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("미지의 숲에 입장하였습니다.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOnCooldown)
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null && playerController.lastMoveInput != Vector2.zero)
            {
                OccurrenceNewEvent();
            }
        }
    }

    private void OccurrenceNewEvent()
    {
        float roll = Random.value; // 0.0 ~ 1.0

        if (roll < 0.33f)
        {
            // 아이템 획득 이벤트
            TryDropItem();
        }
        else if (roll < 0.66f)
        {
            // 전투 발생
            Debug.Log("[미지의 숲] 몬스터가 나타났다! 전투 시작!");
            StartCoroutine(EventCooldown());
        }
        else
        {
            // 아무 일도 없음
            Debug.Log("[미지의 숲] 아무 일도 일어나지 않았다...");
            StartCoroutine(EventCooldown());
        }
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("미지의 숲을 떠났습니다.");
        }
    }
}
