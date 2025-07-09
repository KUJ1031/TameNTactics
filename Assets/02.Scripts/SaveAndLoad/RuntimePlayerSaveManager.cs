using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimePlayerSaveManager : Singleton<RuntimePlayerSaveManager>
{
    public Player playerData;

    // 배틀 직전 호출, 현재 상태를 저장
    public void SaveCurrentGameState(Player player)
    {
        // 각 객체에 복사용 메서드 구현 필요
        player.playerLastGameTime = GameTimeFlow.Instance.GetCurrentTimer();
        player.totalPlaytime += Mathf.FloorToInt(GameTimeFlow.Instance.GetCurrentTimer());
        player.playerLastPosition = PlayerManager.Instance.playerController.transform.position;
        KeyRebinderManager.Instance.SaveCurrentBindingsToPlayer(player);
        playerData = JsonUtility.FromJson<Player>(JsonUtility.ToJson(player)); // 깊은 복사
    }

    public void SaveBattleGameState(Player player)
    {
        playerData = JsonUtility.FromJson<Player>(JsonUtility.ToJson(player));
    }

    // 배틀 종료 후 씬 복귀 시 상태 복원
    public void RestoreGameState()
    {
        if (playerData != null)
        {
            PlayerManager.Instance.player = playerData;
            PlayerManager.Instance.playerController.transform.position = playerData.playerLastPosition;
            GameTimeFlow.Instance.SetTimer(playerData.playerLastGameTime);
            GameTimeFlow.Instance.UpdatePlayTimeText(playerData.totalPlaytime);
        }
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 메모리 데이터 초기화
        playerData = null;
    }
}
