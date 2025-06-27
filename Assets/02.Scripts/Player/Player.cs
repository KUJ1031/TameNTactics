using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    [Header("나의 전체 몬스터")]
    public List<MonsterData> ownedMonsters = new List<MonsterData>();

    [Header("엔트리 등록 몬스터 (최대 5)")]
    public List<MonsterData> entryMonsters = new List<MonsterData>();

    [Header("전투 출전 몬스터 (최대 3)")]
    public List<MonsterData> battleEntry = new List<MonsterData>();

    [Header("벤치 몬스터")]
    public List<MonsterData> benchEntry = new List<MonsterData>();

    public int maxEntryCount = 5;
    public int maxBattleCount = 3;

    [Header("인벤토리/골드")]
    public List<ItemData> items = new List<ItemData>();
    public int gold;

    [Header("기본 정보")]
    public string playerName;
    public int totalPlaytime;
    public int playerLastGameTime;
    public Vector3 playerLastPosition;

    [Header("진행 정보")]
    public Dictionary<int, bool> playerBossClearCheck = new Dictionary<int, bool>();
    public Dictionary<int, bool> playerQuestClearCheck = new Dictionary<int, bool>();
    public Dictionary<int, bool> playerPuzzleClearCheck = new Dictionary<int, bool>();

    [Header("설정 정보")]
    public Dictionary<string, string> playerKeySetting = new Dictionary<string, string>();

    // 몬스터 로스터에 추가
    public bool AddOwnedMonster(MonsterData monster)
    {
        if (ownedMonsters.Contains(monster)) return false;

        ownedMonsters.Add(monster);
        return true;
    }

    // 몬스터 로스터에서 제거
    public bool RealeseMonster(MonsterData monster)
    {
        if (!ownedMonsters.Contains(monster)) return false;

        // 엔트리에 존재하면 제거
        if (entryMonsters.Contains(monster))
        {
            ToggleEntry(monster); // 내부적으로 battleEntry, benchEntry 제거까지 수행
        }

        // 최종적으로 로스터에서 제거
        ownedMonsters.Remove(monster);
        return true;
    }

    // 엔트리 토글 (추가/제거)
    public bool ToggleEntry(MonsterData monster)
    {
        if (!ownedMonsters.Contains(monster)) return false;

        if (entryMonsters.Contains(monster))
        {
            // 제거
            entryMonsters.Remove(monster);
            battleEntry.Remove(monster); // 출전에서도 제거
            UpdateBenchEntry();
            return false; // 제거됨
        }
        else
        {
            // 추가
            if (entryMonsters.Count >= maxEntryCount) return false;

            entryMonsters.Add(monster);
            UpdateBenchEntry();
            return true; // 추가됨
        }
    }

    // 출전 팀 등록 (최대 3명)
    public bool ToggleBattleEntry(MonsterData monster)
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

    // 벤치 리스트 갱신 (entryMonsters 중 battleEntry 제외한 것들)
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
    public bool IsInEntry(MonsterData monster) => entryMonsters.Contains(monster);
    public bool IsInBattle(MonsterData monster) => battleEntry.Contains(monster);

    // 아이템 추가
    public void AddItem(ItemData item)
    {
        // 아이템을 인벤토리에 추가하는 기능
    }

    // 아이템 제거
    public void RemoveItem(ItemData item)
    {
        // 인벤토리에서 아이템을 제거하는 기능
    }

    // 골드 추가
    public void AddGold(int amount)
    {
        // 플레이어의 골드를 증가시키는 기능
    }

    // 골드 사용
    public bool UseGold(int amount)
    {
        // 골드를 사용하고, 사용 성공 여부 반환
        return false;
    }

    // 이름 설정
    public bool SetName(string name)
    {
        // 플레이어 이름을 설정하는 기능
        return false;
    }

    // 마지막 게임 시간 설정
    public void SetPlayerLastGameTime(string time)
    {
        // 마지막 플레이 시간을 설정하는 기능
    }

    // 마지막 위치 저장
    public void SetPlayerLastPosition(string pos)
    {
        // 마지막 플레이 위치를 저장하는 기능
    }

    // 보스 클리어 체크 설정
    public void SetPlayerBossClearCheck(int bossId)
    {
        // 해당 보스 클리어 여부를 true로 설정
    }

    // 퀘스트 클리어 체크 설정
    public void SetPlayerQuestClearCheck(int questId)
    {
        // 해당 퀘스트 클리어 여부를 true로 설정
    }

    // 퍼즐 클리어 체크 설정
    public void SetPlayerPuzzleClearCheck(int puzzleId)
    {
        // 해당 퍼즐 클리어 여부를 true로 설정
    }

    // 키 설정 변경
    public void SetPlayerKeySetting(string keyName, string keyValue)
    {
        // 해당 키 설정 값을 변경
    }


}