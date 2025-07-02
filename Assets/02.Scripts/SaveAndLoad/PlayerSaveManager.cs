using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{
    public void SavePlayerData(Player player)
    {
        // 플레이어 데이터를 저장하는 로직을 구현합니다.
        // Json
        string json = JsonUtility.ToJson(player, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
        Debug.Log("플레이어 데이터 저장: " + PlayerManager.Instance.player);
    }

    public Player LoadPlayerData()
    {
        // 플레이어 데이터를 불러오는 로직을 구현합니다.
        string path = Application.persistentDataPath + "/playerData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            Player player = PlayerManager.Instance.player;
            player = JsonUtility.FromJson<Player>(json);
            Debug.Log("플레이어 데이터 불러오기: " + player);
            Debug.Log("플레이어 데이터 불러오기 경로: " + path);
            Debug.Log("플레이어 전체 몬스터: " + player.ownedMonsters);
            Debug.Log("플레이어 엔트리 몬스터: " + player.entryMonsters);
            Debug.Log("플레이어 전투 출전 몬스터: " + player.battleEntry);
            Debug.Log("플레이어 벤치 몬스터: " + player.benchEntry);
            Debug.Log("플레이어 인벤토리 아이템: " + player.items);
            Debug.Log("플레이어 골드: " + player.gold);
            Debug.Log("플레이어 이름: " + player.playerName);
            Debug.Log("플레이어 총 플레이 시간: " + player.totalPlaytime);
            Debug.Log("플레이어 마지막 게임 시간: " + player.playerLastGameTime);
            Debug.Log("플레이어 마지막 위치: " + player.playerLastPosition);
            Debug.Log("플레이어 보스 클리어 체크: " + player.playerBossClearCheck);
            Debug.Log("플레이어 퀘스트 클리어 체크: " + player.playerQuestClearCheck);
            Debug.Log("플레이어 퍼즐 클리어 체크: " + player.playerPuzzleClearCheck);
            Debug.Log("플레이어 키 설정: " + player.playerKeySetting);
            return player;
        }
        else
        {
            Debug.LogWarning("저장된 플레이어 데이터가 없습니다.");
            return null;
        }
    }
}
