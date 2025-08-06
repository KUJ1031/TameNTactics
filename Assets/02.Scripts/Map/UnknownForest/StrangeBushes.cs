using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class StrangeBushes : MonoBehaviour
{

    public float eventCooldown = 1f;
    private bool isOnCooldown = false;
    private bool playerInZone = false;
    private GameObject player;

    public float respawnDelay = 30f;

    public UnknownForest unknownForest;

    private Dictionary<string, float> itemDropChances = new();

    public List<MonsterData> forestMonsterDataList; // 인스펙터에서 세팅

    private void Start()
    {
        InitializeItemChances();

        if (unknownForest == null)
        {
            unknownForest = FindObjectOfType<UnknownForest>();
            if (unknownForest == null)
            {
                Debug.LogError("[StrangeBushes] UnknownForest 컴포넌트를 찾을 수 없습니다!");
            }
        }
    }
    private void InitializeItemChances()
    {
        if (!PlayerManager.Instance.player.playerQuestStartCheck[1])
        {
            Debug.Log("[StrangeBushes] 레거의 편지 퀘스트가 시작되지 않았습니다. 아이템 드랍 확률을 초기화합니다.");
            itemDropChances["소형 회복 물약"] = 0.4f;
            itemDropChances["중형 회복 물약"] = 0.25f;
            itemDropChances["대형 회복 물약"] = 0.15f;
            itemDropChances["소형 전체 회복 물약"] = 0.1f;
            itemDropChances["중형 전체 회복 물약"] = 0.05f;
            itemDropChances["대형 전체 회복 물약"] = 0.02f;
            itemDropChances["이상한 물약"] = 0.02f;
            itemDropChances["고기"] = 0.01f;
        }
        else
        {
            Debug.Log("[StrangeBushes] 레거의 편지 퀘스트가 시작되었습니다. 아이템 드랍 확률을 업데이트합니다.");
            itemDropChances["소형 회복 물약"] = 0.3f;
            itemDropChances["중형 회복 물약"] = 0.2f;
            itemDropChances["레거의 편지"] = 0.2f;
            itemDropChances["대형 회복 물약"] = 0.1f;
            itemDropChances["소형 전체 회복 물약"] = 0.1f;
            itemDropChances["중형 전체 회복 물약"] = 0.05f;
            itemDropChances["대형 전체 회복 물약"] = 0.02f;
            itemDropChances["이상한 물약"] = 0.02f;
            itemDropChances["고기"] = 0.01f;
        }

    }

    void Update()
    {
        if (playerInZone && !isOnCooldown && Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.Instance.StartDialogue("나", PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender], 7000);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = true;
            player = collision.gameObject;
            UnknownForestManager.Instance.currentBush = this;  // 자신을 등록
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
            player = null;
            UnknownForestManager.Instance.currentBush = null;  // 자신을 해제
        }
    }

    public void OccurrenceNewEvent()
    {
        InitializeItemChances();
        float roll = Random.value;

        if (roll < 1f)
        {
            DialogueManager.Instance.StartDialogue("나", PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender], 7002);

        }
        else if (roll < 0f)
        {
            Debug.Log("[미지의 숲] 몬스터가 나타났다! 전투 시작!");
            DialogueManager.Instance.StartDialogue("나", PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender], 7003);

        }
        else
        {
            Debug.Log("[미지의 숲] 아무 일도 일어나지 않았다...");
            DialogueManager.Instance.StartDialogue("나", PlayerManager.Instance.playerImage[PlayerManager.Instance.player.playerGender], 7004);
            gameObject.SetActive(false);

            UnknownForestManager.Instance.unknownForest.RequestBushRespawn(transform.position, respawnDelay);
        }
    }

    public void TryDropItem()
    {
        float totalWeight = 0f;
        foreach (var kvp in itemDropChances)
        {
            totalWeight += kvp.Value;
        }

        float roll = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var kvp in itemDropChances)
        {
            cumulative += kvp.Value;
            if (roll <= cumulative)
            {
                string itemName = kvp.Key;
                Debug.Log($"[미지의 숲] '{itemName}' 아이템을 획득했습니다!");
                PlayerManager.Instance.player.AddItem(itemName, 1);

                if (itemName == "레거의 편지")
                {
                    TriggerLegerLetterEvent();
                }
                gameObject.SetActive(false);
                UnknownForestManager.Instance.unknownForest.RequestBushRespawn(transform.position, 5f);
                return;
            }
        }
    }

    public void TryBattle()
    {
        var generator = new UnknownForestEnemyTeamGenerator(forestMonsterDataList, 9, 14);
        var enemyTeam = generator.GenerateRandomTeam(Random.Range(1, 4));

        Debug.Log($"[StrangeBushes] 몬스터 {enemyTeam.Count}마리 등장!");

        BattleManager.Instance.enemyTeam = enemyTeam;
        RuntimePlayerSaveManager.Instance.SaveCurrentGameState(PlayerManager.Instance.player); // 현재 플레이어 상태 저장

        SceneManager.sceneLoaded += OnBattleSceneLoaded;

        gameObject.SetActive(false);

        UnknownForestManager.Instance.unknownForest.RequestBushRespawn(transform.position, respawnDelay);

        //씬이동
        SceneManager.LoadScene("BattleScene");
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            GameObject attackObj = GameObject.Find("AttackPosition");
            if (attackObj != null)
            {
                BattleManager.Instance.AttackPosition = attackObj.transform;
            }
            else
            {
                Debug.LogWarning("AttackPosition 오브젝트를 찾지 못했음.");
            }

            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
        }
    }

    // 레거의 편지를 얻었을 때 실행되는 이벤트
    private void TriggerLegerLetterEvent()
    {
        // 원하는 이벤트 로직 (대사, 퀘스트 업데이트 등)
        Debug.Log("[미지의 숲] '레거의 편지'를 획득하여 특별한 이벤트가 발생합니다.");
        PlayerManager.Instance.playerController.isInputBlocked = true; // 플레이어 입력 차단
        DialogueManager.Instance.StartDialogue("나", UnknownForestManager.Instance.npcSprite, 728);
    }

}
