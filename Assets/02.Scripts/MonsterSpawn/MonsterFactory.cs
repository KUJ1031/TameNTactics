using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

//필드에 몬스터 스폰
public class MonsterFactory : MonoBehaviour
{
    [Header("몬스터 프리팹 목록")]
    [SerializeField] private List<MonsterData> monsterDataList;  //스폰 몬스터
    //[SerializeField] private GameObject monsterPrefab;  //몬스터 기본 프리팹

    [Header("몬스터 레벨 범위")]
    [SerializeField] private int minLevel;  //스폰 몬스터 레벨 최소값
    [SerializeField] private int maxLevel;  //스폰 몬스터 레벨 최대값

    [Header("BoxCollider2D로부터 계산된 스폰 영역 (읽기 전용)")]
    [SerializeField, ReadOnly] private float width;
    [SerializeField, ReadOnly] private float height;

    private int maxAttempts = 100; //스폰시 겹치지 않게 시도하는 횟수
    private List<Vector3> usedPositions = new List<Vector3>(); //이미 스폰된 위치들

    private void Start()
    {
        var collider = GetComponentInChildren<BoxCollider2D>();
        if (collider != null)
        {
            width = collider.size.x * transform.localScale.x;
            height = collider.size.y * transform.localScale.y;
            //Debug.Log($"BoxCollider2D 크기 자동 설정됨: width={width}, height={height}");
        }
        else
        {
            Debug.LogWarning("BoxCollider2D를 찾지 못했습니다. Factory 오브젝트에 추가해주세요.");
        }

        SpawnAllMonsters();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerManager.Instance.player.playerBattleTutorialCheck)
        {
            var playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Player entered the exhibit area.");
               
                playerController.isInputBlocked = true;
            }
        }

    }

    //몬스터 스폰
    public void SpawnAllMonsters()
    {
        if (monsterDataList != null && monsterDataList.Count > 0)
        {
            foreach (MonsterData monsterData in monsterDataList)
            {
                Vector3 spawnPos = GetRandomPositionInFactory();
                if (spawnPos == Vector3.zero) continue;

                string prefabPath = $"Units/{monsterData.monsterName}";
                GameObject loadedPrefab = Resources.Load<GameObject>(prefabPath);

                if (loadedPrefab == null)
                {
                    Debug.LogWarning($"프리팹 로드 실패 : 해당 프리팹 -> {prefabPath}");
                    continue;
                }

                GameObject monsterGo = Instantiate(loadedPrefab, spawnPos, Quaternion.identity, transform);
                usedPositions.Add(spawnPos);

                MonsterCharacter newMonster = monsterGo.GetComponent<MonsterCharacter>();
                if (newMonster != null)
                {
                    Monster m = new Monster();
                    int randomLevel = Random.Range(minLevel, maxLevel + 1);

                    m.SetMonsterData(monsterData);
                    m.SetLevel(randomLevel);

                    newMonster.Init(m);
                }

                MonsterMover mover = monsterGo.GetComponent<MonsterMover>();
                if (mover != null)
                {
                    Debug.Log("mover 감지");
                    mover.SetMoveArea(GetComponentInChildren<BoxCollider2D>());
                }
            }
        }
    }

    //스폰장소 랜덤 생성
    private Vector3 GetRandomPositionInFactory()
    {
        Vector3 center = transform.position;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(-width / 2f, width / 2f);
            float y = Random.Range(-height / 2f, height / 2f);

            Vector3 candidate = center + new Vector3(x, y, 0f);

            bool isTooClose = usedPositions.Exists(pos => Vector3.Distance(pos, candidate) < 1.5f);
            if (!isTooClose)
                return candidate;
        }

        return Vector3.zero;
    }

    //특정 몬스터를 포함한 랜덤 몬스터리스트 생성
    public List<Monster> GetRandomEnemyTeam(Monster monster)
    {
        List<Monster> selectedTeam = new List<Monster>();

        //충돌한 몬스터 포함
        if (monster != null)
        {
            selectedTeam.Add(monster);
        }

        //추가로 넣을 몬스터 개수 (0,1,2)
        int moreAddMonsterCount = Random.Range(0, 3);
        for (int i = 0; i < moreAddMonsterCount; i++)
        {
            //종류도 랜덤으로 추가
            int randomMonster = Random.Range(0, monsterDataList.Count);

            //몬스터 클래스 생성
            Monster m = new Monster();
            m.SetMonsterData(monsterDataList[randomMonster]);
            selectedTeam.Add(m);
        }

        //레벨설정
        foreach (Monster m in selectedTeam)
        {
            m.SetLevel(Random.Range(minLevel, maxLevel + 1));
        }

        return selectedTeam;
    }
    public List<Monster> GetFixedEnemyTeam(Monster monster)
    {
        List<Monster> selectedTeam = new List<Monster>();

        //충돌한 몬스터 포함
        if (monster != null)
        {
            selectedTeam.Add(monster);
        }
        //레벨설정
        foreach (Monster m in selectedTeam)
        {
            m.SetLevel(Random.Range(minLevel, maxLevel + 1));
        }

        return selectedTeam;
    }
}
