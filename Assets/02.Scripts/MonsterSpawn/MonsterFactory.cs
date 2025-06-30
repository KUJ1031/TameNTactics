using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

//필드에 몬스터 스폰
public class MonsterFactory : MonoBehaviour
{
    [Header("몬스터 프리팹 목록")]
    [SerializeField] private List<MonsterData> monsterDataList;  //스폰 몬스터
    [SerializeField] private GameObject monsterPrefab;  //몬스터 기본 프리팹

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
            Debug.Log($"BoxCollider2D 크기 자동 설정됨: width={width}, height={height}");
        }
        else
        {
            Debug.LogWarning("BoxCollider2D를 찾지 못했습니다. Factory 오브젝트에 추가해주세요.");
        }

        SpawnAllMonsters();
    }

    //몬스터 스폰
    public void SpawnAllMonsters()
    {
        if (monsterDataList != null && monsterDataList.Count > 0)
        {
            foreach (MonsterData monsterData in monsterDataList)
            {
                //스폰위치 생성(실패시 Vector3.zero 반환)
                Vector3 spawnPos = GetRandomPositionInFactory();

                if (spawnPos != Vector3.zero)
                {
                    //위치에 몬스터 생성
                    GameObject monsterGO = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
                    usedPositions.Add(spawnPos);

                    //만들어진 기본몬스터의 데이터를  monsterList안의 값으로 변경
                    Monster newMonster = monsterGO.GetComponent<Monster>();
                    if (monsterData != null)
                    {
                        newMonster.SetMonsterData(monsterData);

                        int randomLevel = Random.Range(minLevel, maxLevel + 1);
                        newMonster.SetLevel(randomLevel); // 레벨 설정 메서드 호출
                        // monster.monsterData.level = randomLevel; // 혹시 Stat 초기화할 때 사용한다면
                        // monster.LoadMonsterBaseStatData();       // level 반영된 Stat 적용
                    }

                    // 이동 영역 설정
                    MonsterMover mover = monsterGO.GetComponent<MonsterMover>();
                    if (mover != null)
                    {
                        mover.SetMoveArea(GetComponentInChildren<BoxCollider2D>());
                    }

                    Debug.Log($"{monsterGO.name} 생성 완료 @ {spawnPos}, 레벨: {newMonster.Level}");
                }
                else { Debug.Log("몬스터 생성 실패 : Vector3.zero"); }
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
            GameObject dummy = new GameObject();
            Monster m = dummy.AddComponent<Monster>();
            m.SetMonsterData(monsterDataList[randomMonster]);
         
            selectedTeam.Add(m);
            Destroy(dummy);
        }

        //레벨설정
        foreach (Monster m in selectedTeam)
        {
            m.Level = Random.Range(minLevel, maxLevel + 1);
        }

        return selectedTeam;
    }
}
