using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class RuntimePlayerSaveManager : Singleton<RuntimePlayerSaveManager>
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public float[] position;
        public float lastGameTime;
        public int totalPlaytime;
        public float sceneTransitionStartTime; // 추가
    }

    public Player playerData;
    private float sceneTransitionStartTime;

    private string savePath => Application.persistentDataPath + "/player_save.json";

    public void SaveCurrentGameState(Player player)
    {
        // 저장 직전에 타이머 값 갱신
        float currentGameTime = GameTimeFlow.Instance.GetCurrentTimer();

        PlayerSaveData saveData = new PlayerSaveData
        {
            position = new float[]
            {
            PlayerManager.Instance.playerController.transform.position.x,
            PlayerManager.Instance.playerController.transform.position.y,
            PlayerManager.Instance.playerController.transform.position.z
            },
            lastGameTime = currentGameTime,
            totalPlaytime = player.totalPlaytime + Mathf.FloorToInt(currentGameTime),
            sceneTransitionStartTime = sceneTransitionStartTime
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }


    public void RestoreGameState()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerSaveData playerData = JsonUtility.FromJson<PlayerSaveData>(json);

            Vector3 restoredPosition = new Vector3(
                playerData.position[0],
                playerData.position[1],
                playerData.position[2]
            );
            PlayerManager.Instance.playerController.transform.position = restoredPosition;
            float sceneElapsed = GameTimeFlow.Instance.GetCurrentTimer() - playerData.sceneTransitionStartTime;
            float updatedGameTime = playerData.lastGameTime + sceneElapsed;

            GameTimeFlow.Instance.SetTimer(updatedGameTime);
            GameTimeFlow.Instance.UpdatePlayTimeText(playerData.totalPlaytime);
        }
    }

    public void SaveBattleGameState(Player player)
    {
        playerData = JsonUtility.FromJson<Player>(JsonUtility.ToJson(player));
    }

    public bool HasSavedData()
    {
        string savePath = Application.persistentDataPath + "/player_save.json";
        return playerData != null && File.Exists(savePath);
    }

    private void OnApplicationQuit()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("게임 종료: 저장된 JSON 파일 삭제됨.");
        }
    }
}
