using System.Collections.Generic;
using UnityEngine;

public class EntryManager : Singleton<EntryManager>
{
    [Header("최대 출전 수")]
    public int maxEntryCount = 3;

    [Header("ScrollView 관련")]
    public Transform contentPanel;
    public GameObject monsterSlotPrefab;

    public event System.Action OnEntryChanged;

    private void Start()
    {
        InitializeAllSlots();
    }

    public void InitializeAllSlots()
    {
        Debug.Log("[EntryManager] InitializeAllSlots 호출됨");
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        var entryCandidates = PlayerManager.Instance.player.entryMonsters;
        Debug.Log($"entryCandidates.Count = {entryCandidates.Count}");

        foreach (var monster in entryCandidates)
        {
            GameObject slot = Instantiate(monsterSlotPrefab, contentPanel);
            slot.GetComponent<MonsterSlot>().SetData(monster);
        }
    }

    public void ToggleEntry(MonsterData monster)
    {
        Debug.Log("[EntryManager] ToggleEntry 호출됨");
        var player = PlayerManager.Instance.player;

        if (!player.IsInEntry(monster)) return;

        if (player.IsInBattle(monster))
        {
            if (player.battleEntry.Count <= 1)
            {
                Debug.LogWarning("최소 1마리는 출전해야 합니다.");
                return;
            }

            player.ToggleBattleEntry(monster); // 해제
        }
        else
        {
            if (player.battleEntry.Count >= maxEntryCount)
            {
                Debug.LogWarning("최대 출전 수 초과");
                return;
            }

            player.ToggleBattleEntry(monster); // 등록
        }
        InitializeAllSlots();
        OnEntryChanged?.Invoke();
    }

    public bool IsInEntry(MonsterData monster)
    {
        return PlayerManager.Instance.player.IsInBattle(monster);
    }

    [Header("출전 후보 최대 수 (최대 5)")]
    public int maxCandidateCount = 5;

    public bool ToggleCandidate(MonsterData monster)
    {
        var player = PlayerManager.Instance.player;

        if (!player.ownedMonsters.Contains(monster))
            return false;

        bool result = player.ToggleEntry(monster);

        if (result) // 추가 성공
        {
            InitializeAllSlots();  // UI 갱신
        }
        else if (!player.entryMonsters.Contains(monster)) // 제거된 경우
        {
            InitializeAllSlots();  // 제거 후에도 UI 갱신 필요
        }

        OnEntryChanged?.Invoke();
        return result;
    }
}
