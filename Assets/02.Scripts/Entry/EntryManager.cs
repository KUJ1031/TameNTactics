using System.Collections.Generic;
using UnityEngine;

public class EntryManager : Singleton<EntryManager>
{
    [Header("최대 출전 수")]
    public int maxEntryCount = 3;

    [Header("현재 출전 리스트")]
    public List<MonsterData> allMonsters = new List<MonsterData>(); // 출전 후보군, 시작 시 빈 리스트
    public List<MonsterData> selectedEntries = new List<MonsterData>();

    public event System.Action OnEntryChanged;

    protected override void Awake()
    {
        base.Awake();
        if (selectedEntries.Count == 0 && allMonsters.Count > 0)
        {
            selectedEntries.Add(allMonsters[0]);
            OnEntryChanged?.Invoke();
        }
    }

    private void Start()
    {
        InitializeAllSlots();
    }

    void InitializeAllSlots()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        foreach (var monster in allMonsters)
        {
            GameObject slot = Instantiate(monsterSlotPrefab, contentPanel);
            slot.GetComponent<MonsterSlot>().SetData(monster);
        }
    }

    public void ToggleEntry(MonsterData monster)
    {
        bool isSelected = selectedEntries.Contains(monster);

        if (isSelected)
        {
            if (selectedEntries.Count <= 1)
            {
                Debug.LogWarning("최소 1마리는 출전해야 합니다.");
                return;
            }
            selectedEntries.Remove(monster);
        }
        else
        {
            if (selectedEntries.Count >= maxEntryCount)
            {
                Debug.LogWarning("최대 출전 수 초과");
                return;
            }
            selectedEntries.Add(monster);
        }

        OnEntryChanged?.Invoke();
    }

    public bool IsInEntry(MonsterData monster)
    {
        return selectedEntries.Contains(monster);
    }

    // 아래 2개 변수와 setter는 새로 추가 (출전 후보군 최대 5명 제한)
    [Header("출전 후보 최대 수 (최대 5)")]
    public int maxCandidateCount = 5;

    /// <summary>
    /// 출전 후보군에 몬스터 추가
    /// </summary>
    public bool AddCandidate(MonsterData monster)
    {
        if (allMonsters.Contains(monster))
            return false;

        if (allMonsters.Count >= maxCandidateCount)
        {
            Debug.LogWarning("출전 후보군 최대 수 초과");
            return false;
        }

        allMonsters.Add(monster);
        InitializeAllSlots();  // UI 갱신
        return true;
    }

    /// <summary>
    /// 출전 후보군에서 몬스터 제거
    /// </summary>
    public void RemoveCandidate(MonsterData monster)
    {
        if (allMonsters.Contains(monster))
        {
            allMonsters.Remove(monster);

            // 출전 상태면 해제
            if (selectedEntries.Contains(monster))
                selectedEntries.Remove(monster);

            InitializeAllSlots();
            OnEntryChanged?.Invoke();
        }
    }

    [Header("ScrollView 관련")]
    public Transform contentPanel; // ScrollView Content (기존에 있던 변수)
    public GameObject monsterSlotPrefab; // 몬스터 슬롯 프리팹 (기존에 있던 변수)
}
