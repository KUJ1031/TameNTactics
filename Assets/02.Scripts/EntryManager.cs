using System;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : Singleton<EntryManager>
{
    [Header("최대 출전 수")]
    public int maxEntryCount = 3;

    [Header("현재 출전 리스트")]
    public List<MonsterData> selectedEntries = new List<MonsterData>();

    // Entry에 몬스터 추가/해제 시에 사용될 이벤트
    public event Action OnEntryChanged;

    // 몬스터를 엔트리에 추가 또는 해제
    public void ToggleEntry(MonsterData monster)
    {
        if (selectedEntries.Contains(monster))
        {
            selectedEntries.Remove(monster);
            Debug.Log($"{monster.monsterName} 출전 해제");
        }
        else
        {
            if (selectedEntries.Count >= maxEntryCount)
            {
                Debug.LogWarning("최대 출전 수가 초과");
                return;
            }

            selectedEntries.Add(monster);
            Debug.Log($"{monster.monsterName} 출전 등록");
        }

        // UI 업데이트 등 후처리 (필요 시)
        OnEntryChanged?.Invoke();
    }

    // 현재 출전 여부 확인
    public bool IsInEntry(MonsterData monster)
    {
        return selectedEntries.Contains(monster);
    }
}
