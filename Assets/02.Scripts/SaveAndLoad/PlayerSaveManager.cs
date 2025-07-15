using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;


using static RuntimePlayerSaveManager;

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public List<Monster> ownedMonsters = new(); // 이건 풀 데이터 저장
        public List<int> entryMonsterIDs = new();   // entryMonsters는 ID만 저장
        public List<int> battleMonsterIDs = new();
        public List<int> benchMonsterIDs = new();

        // ...기타 다른 필드들
    }
    public void SavePlayerData(Player player)
    {
        var flow = GameTimeFlow.Instance;
        player.playerLastGameTime = flow.GetCurrentTimer();
        player.totalPlaytime += Mathf.FloorToInt(flow.GetCurrentTimer());
        player.playerLastPosition = PlayerManager.Instance.playerController.transform.position;

        KeyRebinderManager.Instance.SaveCurrentBindingsToPlayer(player);

        var saveData = new PlayerSaveData
        {
            ownedMonsters = player.ownedMonsters,
            entryMonsterIDs = player.entryMonsters.Select(m => m.monsterID).ToList(),
            battleMonsterIDs = player.battleEntry.Select(m => m.monsterID).ToList(),
            benchMonsterIDs = player.benchEntry.Select(m => m.monsterID).ToList(),
            // 다른 필드들 복사...
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

    public Player LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (!File.Exists(path))
        {
            Debug.LogWarning("저장된 플레이어 데이터가 없습니다.");
            return null;
        }

        string json = File.ReadAllText(path);
        PlayerSaveData saved = JsonUtility.FromJson<PlayerSaveData>(json);

        Player player = new Player();
        player.ownedMonsters = saved.ownedMonsters;

        // ID 기반으로 entry 복원
        player.entryMonsters = player.ownedMonsters
            .Where(mon => saved.entryMonsterIDs.Contains(mon.monsterID))
            .ToList();

        player.battleEntry = player.ownedMonsters
            .Where(mon => saved.battleMonsterIDs.Contains(mon.monsterID))
            .ToList();

        player.benchEntry = player.ownedMonsters
            .Where(mon => saved.benchMonsterIDs.Contains(mon.monsterID))
            .ToList();

        // 기타 필드 복원...

        return player;
    }


    // 저장 버튼에 연결할 메서드: 저장 기능 호출만 담당
    public void OnSaveButtonPressed()
    {
        Debug.Log("플레이어 현재 위치 저장 시점: " + PlayerManager.Instance.playerController.transform.position);
        SavePlayerData(PlayerManager.Instance.player);
    }

    // 불러오기 버튼에 연결할 메서드: 불러온 데이터를 적용하는 로직을 분리
    public void OnLoadButtonPressed()
    {
        Player loaded = LoadPlayerData();
        if (loaded != null)
        {
            ApplyLoadedPlayerData(loaded);
            Debug.Log("플레이어 데이터 로드 완료, 위치 및 시간 적용됨");
        }
    }

    // 불러온 플레이어 데이터를 게임 상태에 적용하는 함수
    private void ApplyLoadedPlayerData(Player loaded)
    {
        PlayerManager.Instance.player = loaded;

        // 시간 적용
        GameTimeFlow.Instance.SetTimer(loaded.playerLastGameTime);
        GameTimeFlow.Instance.UpdatePlayTimeText(loaded.totalPlaytime);

        // 위치 적용
        if (PlayerManager.Instance.playerController != null)
        {
            PlayerManager.Instance.playerController.transform.position = loaded.playerLastPosition;
        }
        else
        {
            Debug.LogWarning("playerController가 할당되지 않아 위치를 적용하지 못했습니다.");
        }
    }
}
