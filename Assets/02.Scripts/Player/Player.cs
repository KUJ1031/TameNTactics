using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    [Header("나의 전체 몬스터")]
    public List<Monster> ownedMonsters = new();

    [Header("엔트리 등록 몬스터 (최대 5)")]
    public List<Monster> entryMonsters = new();

    [Header("전투 출전 몬스터 (최대 3)")]
    public List<Monster> battleEntry = new();

    [Header("벤치 몬스터")]
    public List<Monster> benchEntry = new();

    public int maxEntryCount = 5;
    public int maxBattleCount = 3;

    [Header("인벤토리/골드")]
    public List<ItemData> items = new List<ItemData>();
    public int gold;

    [Header("기본 정보")]
    public string playerName;
    public int totalPlaytime;
    public float playerLastGameTime;
    public Vector3 playerLastPosition;

    [Header("진행 정보")]
    public SerializableDictionary<int, bool> playerBossClearCheck = new();
    public SerializableDictionary<int, bool> playerQuestClearCheck = new();
    public SerializableDictionary<int, bool> playerPuzzleClearCheck = new();

    [Header("설정 정보")]
    public SerializableDictionary<string, string> playerKeySetting = new();

    // Owned에 추가
    public bool AddOwnedMonster(Monster monster)
    {
        if (monster == null || monster.monsterData == null)
        {
            return false;
        }
        ownedMonsters.Add(monster);
        return true;

        
    }

    //Owned에서 제거
    public bool ReleaseMonster(Monster monster)
    {
        if (!ownedMonsters.Contains(monster)) return false;

        if (entryMonsters.Contains(monster))
            ToggleEntry(monster);

        ownedMonsters.Remove(monster);
        return true;
    }

    // 엔트리 토글
    public bool ToggleEntry(Monster monster)
    {
        if (!ownedMonsters.Contains(monster)) return false;

        if (entryMonsters.Contains(monster))
        {
            entryMonsters.Remove(monster);
            battleEntry.Remove(monster);
            UpdateBenchEntry();
            return false;
        }
        else
        {
            if (entryMonsters.Count >= maxEntryCount) return false;

            entryMonsters.Add(monster);
            UpdateBenchEntry();
            return true;
        }
    }

    // 출전 토글
    public bool ToggleBattleEntry(Monster monster)
    {
        if (!entryMonsters.Contains(monster)) return false;

        if (battleEntry.Contains(monster))
        {
            battleEntry.Remove(monster);
            UpdateBenchEntry();
            return true;
        }
        else
        {
            if (battleEntry.Count >= maxBattleCount) return false;

            battleEntry.Add(monster);
            UpdateBenchEntry();
            return true;
        }
    }

    // 벤치 갱신
    private void UpdateBenchEntry()
    {
        benchEntry.Clear();
        foreach (var m in entryMonsters)
        {
            if (!battleEntry.Contains(m))
                benchEntry.Add(m);
        }
    }

    // 출전 상태 확인
    public bool IsInEntry(Monster monster) => entryMonsters.Contains(monster);
    public bool IsInBattle(Monster monster) => battleEntry.Contains(monster);

    // MonsterData 기준으로 검색할 때 사용
    public Monster FindMonsterByData(MonsterData data)
    {
        return ownedMonsters.Find(m => m.monsterData == data);
    }

    // 아이템 추가
    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    // 아이템 제거
    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }

    // 골드 추가
    public void AddGold(int amount)
    {
        gold += amount;
    }

    // 골드 사용
    public bool UseGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        return true;
    }

    // 이름 설정
    public bool SetName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        playerName = name;
        return true;
    }

    public void SetPlayerLastGameTime(string time)
    {
        if (int.TryParse(time, out var result))
            playerLastGameTime = result;
    }

    public void SetPlayerLastPosition(string pos)
    {
        var split = pos.Split(',');
        if (split.Length != 3) return;

        if (float.TryParse(split[0], out float x) &&
            float.TryParse(split[1], out float y) &&
            float.TryParse(split[2], out float z))
        {
            playerLastPosition = new Vector3(x, y, z);
        }
    }

    public void SetPlayerBossClearCheck(int bossId)
    {
        playerBossClearCheck[bossId] = true;
    }

    public void SetPlayerQuestClearCheck(int questId)
    {
        playerQuestClearCheck[questId] = true;
    }

    public void SetPlayerPuzzleClearCheck(int puzzleId)
    {
        playerPuzzleClearCheck[puzzleId] = true;
    }

    public void SetPlayerKeySetting(string keyName, string keyValue)
    {
        playerKeySetting[keyName] = keyValue;
    }
}
