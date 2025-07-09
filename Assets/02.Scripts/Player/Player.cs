using System;
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

    [Header("벤치 몬스터")]//(entryMonsters-battleEntry)
    public List<Monster> benchEntry = new();

    public int maxEntryCount = 5;
    public int maxBattleCount = 3;

    [Header("인벤토리/골드")]
    public List<ItemInstance> items = new List<ItemInstance>();
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


    //monster Null 체크
    private bool CheckMonster(Monster monster)
    {
        return monster != null && monster.monsterData != null;
    }

    // Owned에 추가
    public bool AddOwnedMonster(Monster monster)
    {
        if (!CheckMonster(monster)) return false;

        ownedMonsters.Add(monster);
        return true;
    }

    //Owned에서 제거
    public bool RemoveOwnedMonster(Monster monster)
    {
        if (!CheckMonster(monster)) return false;

        if (!ownedMonsters.Remove(monster))
            return false;

        // Entry에 있으면 제거하고 Battle/Bench에서도 제거
        if (entryMonsters.Remove(monster))
        {
            battleEntry.Remove(monster);
            benchEntry.Remove(monster);
        }

        return true;
    }

    //Entry에 추가
    public void TryAddEntryMonster(Monster monster, Action<bool> onCompleted)
    {
        if (!CheckMonster(monster))
        {
            onCompleted?.Invoke(false);
            return;
        }

        if (entryMonsters.Contains(monster))
        {
            onCompleted?.Invoke(false);
            return;
        }

        if (entryMonsters.Count < maxEntryCount)
        {
            entryMonsters.Add(monster);

            if (battleEntry.Count < maxBattleCount)
                battleEntry.Add(monster);
            else
                benchEntry.Add(monster);

            onCompleted?.Invoke(true);
        }
        else
        {
            //UI호출 및 대기
            FieldUIManager.Instance.SwapEntryMonster((swapMonster) =>
            {
                if (swapMonster == null || !entryMonsters.Contains(swapMonster))
                {
                    onCompleted?.Invoke(false);
                    return;
                }

                entryMonsters.Remove(swapMonster);
                entryMonsters.Add(monster);

                if (battleEntry.Contains(swapMonster))
                {
                    battleEntry.Remove(swapMonster);
                    battleEntry.Add(monster);
                }
                else
                {
                    benchEntry.Remove(swapMonster);
                    benchEntry.Add(monster);
                }

                onCompleted?.Invoke(true);
            });
        }
    }

    //Entry에 제거
    public bool RemoveEntryMonster(Monster monster)
    {
        if (!CheckMonster(monster)) return false;
        if (!entryMonsters.Contains(monster)) return false;

        if (battleEntry.Remove(monster))
        {
            if (benchEntry.Count > 0)
            {
                Monster promoted = benchEntry[0];
                benchEntry.RemoveAt(0);
                battleEntry.Add(promoted);
            }
        }
        else
        {
            benchEntry.Remove(monster);
        }

        return true;
    }

    // benchEntry -> battleEntry 이동
    public bool AddBattleEntry(Monster monster)
    {
        if (!CheckMonster(monster)) return false;
        if (battleEntry.Contains(monster)) return false;
        if (!entryMonsters.Contains(monster)) return false;
        if (battleEntry.Count >= maxBattleCount) return false;

        // 벤치에 있었다면 제거
        benchEntry.Remove(monster);

        battleEntry.Add(monster);
        return true;
    }

    // battleEntry -> benchEntry 이동
    public bool RemoveBattleEntry(Monster monster)
    {
        if (!CheckMonster(monster)) return false;
        if (!battleEntry.Remove(monster)) return false;

        // 벤치로 내림
        benchEntry.Add(monster);
        return true;
    }

    //몬스터 위치 변경 (battleEntry <-> benchEntry)(엔트리UI에서 쓸거)
    public bool InsertWithSwapIfFull(List<Monster> fromList, List<Monster> toList, Monster monster, int toIndex)
    {
        if (!CheckMonster(monster)) return false;
        if (!fromList.Contains(monster)) return false;
        if (toList.Contains(monster)) return false;

        // 몬스터를 출발 리스트에서 제거
        fromList.Remove(monster);

        // toList가 가득 찼다면 밀려날 몬스터를 빼서 fromList로 이동
        if (toList.Count >= maxBattleCount)
        {
            // 밀릴 몬스터 선정: 맨 마지막 or toIndex 위치
            int removeIndex = Mathf.Clamp(toIndex, 0, toList.Count - 1);
            Monster displaced = toList[removeIndex];
            toList.RemoveAt(removeIndex);
            fromList.Add(displaced);
        }

        // toIndex에 삽입 (정확한 위치로)
        toIndex = Mathf.Clamp(toIndex, 0, toList.Count);
        toList.Insert(toIndex, monster);

        return true;
    }

    //전체 엔트리 초기화
    public void ClearEntry()
    {
        entryMonsters.Clear();
        battleEntry.Clear();
        benchEntry.Clear();
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
    public void AddItem(ItemInstance item)
    {
        items.Add(item);
    }

    // 아이템 제거
    public void RemoveItem(ItemInstance item)
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
