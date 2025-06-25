using System.Collections.Generic;
using UnityEngine;

public class EntryManager : Singleton<EntryManager>
{
    [Header("최대 출전 수")]
    public int maxEntryCount = 3;

    [Header("현재 출전 리스트")]
    public List<MonsterData> allMonsters; // 에디터에서 5개 연결
    public List<MonsterData> selectedEntries = new List<MonsterData>();

    [Header("ScrollView 관련")]
    public Transform contentPanel; // Content 객체
    public GameObject monsterSlotPrefab; // MonsterSlot 프리팹

    public event System.Action OnEntryChanged;

    void Start()
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
        if (selectedEntries.Contains(monster))
        {
            selectedEntries.Remove(monster);
            Debug.Log($"{monster.monsterName} 출전 해제");
        }
        else
        {
            if (selectedEntries.Count >= maxEntryCount)
            {
                Debug.LogWarning("최대 출전 수 초과");
                return;
            }

            selectedEntries.Add(monster);
            Debug.Log($"{monster.monsterName} 출전 등록");
        }

        // UI 갱신
        OnEntryChanged?.Invoke();
    }

    public bool IsInEntry(MonsterData monster)
    {
        return selectedEntries.Contains(monster);
    }
}
