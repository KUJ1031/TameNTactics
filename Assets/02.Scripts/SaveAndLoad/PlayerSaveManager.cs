using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Text;

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{

    protected override bool IsDontDestroy => true;

    [System.Serializable]
    public class PlayerSaveData
    {
        public int saveVersion = 1; // 향후 버전 관리용
        public List<MonsterSaveData> ownedMonsters = new();
        public List<int> entryMonsterIDs = new();   // entryMonsters는 ID만 저장
        public List<int> battleMonsterIDs = new();
        public List<int> benchMonsterIDs = new();
        public List<ItemInstanceSaveData> items = new();
        public int gold;
        public float playerLastGameTime;
        public Vector3 playerLastPosition;
        public string playerLastStage;
        public int totalPlaytime;
        public string playerName;
        public int playerGetMonsterCount;
        public int playerGender;
        public List<ItemInstanceSaveData> playerEquipment = new(); // 장착 아이템 목록
        public bool playerBattleTutorialCheck = false; // 튜토리얼 클리어 여부
        public bool playerAllTutorialCheck = false; // 전체 튜토리얼 클리어 여부
        public SerializableDictionary<int, bool> playerBossClearCheck = new();
        public SerializableDictionary<int, bool> playerQuestStartCheck= new();
        public SerializableDictionary<int, bool> playerQuestClearCheck = new();
        public SerializableDictionary<int, bool> playerEliteStartCheck = new(); // 엘리트 던전 클리어 여부
        public SerializableDictionary<int, bool> playerEliteClearCheck = new(); // 퍼즐 클리어 여부
        public SerializableDictionary<int, bool> playerPuzzleClearCheck = new();
        public SerializableDictionary<string, string> playerKeySetting = new();


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
            saveVersion = 1,
            ownedMonsters = player.ownedMonsters.Select(m => m.ToSaveData()).ToList(),
            entryMonsterIDs = player.entryMonsters.Select(m => m.monsterID).ToList(),
            battleMonsterIDs = player.battleEntry.Select(m => m.monsterID).ToList(),
            benchMonsterIDs = player.benchEntry.Select(m => m.monsterID).ToList(),

            // ItemInstance → ItemInstanceSaveData 변환
            items = player.items.Select(i => i.ToSaveData()).ToList(),

            gold = player.gold,
            playerLastGameTime = player.playerLastGameTime,
            playerLastPosition = player.playerLastPosition,
            playerLastStage = player.playerLastStage,
            totalPlaytime = player.totalPlaytime,
            playerName = player.playerName,
            playerGender = player.playerGender,

            // playerEquipment도 변환 필요
            playerEquipment = player.playerEquipment.Select(i => i.ToSaveData()).ToList(),

            playerGetMonsterCount = player.playerGetMonsterCount,
            playerBattleTutorialCheck = player.playerBattleTutorialCheck,
            playerAllTutorialCheck = player.playerAllTutorialCheck,
            playerBossClearCheck = player.playerBossClearCheck,
            playerQuestStartCheck = player.playerQuestStartCheck,
            playerQuestClearCheck = player.playerQuestClearCheck,
            playerEliteStartCheck = player.playerEliteStartCheck,
            playerEliteClearCheck = player.playerEliteClearCheck,
            playerPuzzleClearCheck = player.playerPuzzleClearCheck,
            playerKeySetting = player.playerKeySetting
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
        EventAlertManager.Instance.SetEventAlert(EventAlertType.Save);
    }


    public Player LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (!File.Exists(path))
        {
            Debug.LogWarning("저장된 플레이어 데이터가 없습니다.");
            return null;
        }
        Debug.Log("플레이어 데이터 로드 경로: " + path);

        string json = File.ReadAllText(path, Encoding.UTF8);
        PlayerSaveData saved = JsonUtility.FromJson<PlayerSaveData>(json);

        Player player = new Player();
        player.ownedMonsters = saved.ownedMonsters
            .Select(sd => Monster.CreateFromSaveData(sd, MonsterDatabase.Instance, SkillDatabase.Instance))
            .ToList();

        player.entryMonsters = player.ownedMonsters
            .Where(mon => saved.entryMonsterIDs.Contains(mon.monsterID))
            .ToList();

        player.battleEntry = player.ownedMonsters
            .Where(mon => saved.battleMonsterIDs.Contains(mon.monsterID))
            .ToList();

        player.benchEntry = player.ownedMonsters
            .Where(mon => saved.benchMonsterIDs.Contains(mon.monsterID))
            .ToList();

        // ItemInstanceSaveData → ItemInstance 변환
        player.items = saved.items
            .Select(sd => ItemInstance.FromSaveData(sd, ItemDatabase.Instance))
            .Where(i => i != null)
            .ToList();

        player.gold = saved.gold;
        player.playerLastGameTime = saved.playerLastGameTime;
        player.playerLastPosition = saved.playerLastPosition;
        player.playerLastStage = saved.playerLastStage;
        player.totalPlaytime = saved.totalPlaytime;
        player.playerName = saved.playerName;
        player.playerGender = saved.playerGender;

        player.playerEquipment = saved.playerEquipment
            .Select(sd => ItemInstance.FromSaveData(sd, ItemDatabase.Instance))
            .Where(i => i != null)
            .ToList();

        player.playerGetMonsterCount = saved.playerGetMonsterCount;
        player.playerBattleTutorialCheck = saved.playerBattleTutorialCheck;
        player.playerAllTutorialCheck = saved.playerAllTutorialCheck;
        player.playerBossClearCheck = saved.playerBossClearCheck;
        player.playerQuestStartCheck = saved.playerQuestStartCheck;
        player.playerQuestClearCheck = saved.playerQuestClearCheck;
        player.playerEliteStartCheck = saved.playerEliteStartCheck;
        player.playerEliteClearCheck = saved.playerEliteClearCheck;
        player.playerPuzzleClearCheck = saved.playerPuzzleClearCheck;
        player.playerKeySetting = saved.playerKeySetting;

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
