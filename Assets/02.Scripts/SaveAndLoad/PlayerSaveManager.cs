using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{
    public void SavePlayerData(Player player)
    {
        var flow = GameTimeFlow.Instance;
        player.playerLastGameTime = flow.GetCurrentTimer();
        player.totalPlaytime += Mathf.FloorToInt(flow.GetCurrentTimer());

        if (PlayerManager.Instance.playerController != null)
        {
            player.playerLastPosition = PlayerManager.Instance.playerController.transform.position;
        }
        else
        {
            Debug.LogWarning("playerController가 할당되지 않아 위치를 저장하지 못했습니다.");
        }

        string json = JsonUtility.ToJson(player, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

    public Player LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            Player loadedPlayer = JsonUtility.FromJson<Player>(json);
            return loadedPlayer;
        }
        else
        {
            Debug.LogWarning("저장된 플레이어 데이터가 없습니다.");
            return null;
        }
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
