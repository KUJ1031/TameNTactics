using System.Collections.Generic;
using UnityEngine;

public class MonsterRosterManager : MonoBehaviour
{
    public static MonsterRosterManager Instance;

    [Header("보유 몬스터 리스트")]
    public List<MonsterData> ownedMonsters = new List<MonsterData>();

    [Header("UI 세팅")]
    public Transform rosterContent;
    public GameObject rosterSlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeRoster();
    }

    public void InitializeRoster()
    {
        foreach (Transform child in rosterContent)
            Destroy(child.gameObject);

        foreach (var monster in ownedMonsters)
        {
            GameObject slotObj = Instantiate(rosterSlotPrefab, rosterContent);
            var slot = slotObj.GetComponent<MonsterRosterSlot>();
            slot.SetData(monster);
        }
    }

    /// <summary>
    /// 몬스터 추가 (예: 게임 데이터에서 불러올 때)
    /// </summary>
    public void AddMonster(MonsterData monster)
    {
        if (!ownedMonsters.Contains(monster))
        {
            ownedMonsters.Add(monster);
            InitializeRoster();
        }
    }

    /// <summary>
    /// 몬스터 제거 (방출)
    /// </summary>
    public void RemoveMonster(MonsterData monster)
    {
        if (ownedMonsters.Contains(monster))
        {
            ownedMonsters.Remove(monster);

            // 출전 후보군에서도 제거 시도
            EntryManager.Instance.RemoveCandidate(monster);

            InitializeRoster();
        }
    }

    /// <summary>
    /// 출전 후보군에 등록 요청
    /// </summary>
    public bool RequestAddToEntryCandidates(MonsterData monster)
    {
        return EntryManager.Instance.AddCandidate(monster);
    }
}
