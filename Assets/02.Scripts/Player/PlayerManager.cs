using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("플레이어 데이터 및 컨트롤")]
    public Player player;                       //플레이어 데이터
    public PlayerController playerController;   // 플레이어 조작 클래스
    public GameObject playerPrefab;             //실제 플레이어

    public List<MonsterData> testMonsterList; //테스트용 플레이어 몬스터들(추후 삭제)


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SpawnPlayerCharacter(player);

        //// 1. 저장된 데이터가 있는지 시도해서 불러옴
        //Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();

        //if (loadedPlayer != null)
        //{
        //    // 2. 불러온 데이터를 현재 플레이어에 반영
        //    player = loadedPlayer;
        //    PlayerManager.Instance.player = loadedPlayer;
        //    Debug.Log("저장된 플레이어 데이터를 불러왔습니다.");
        //}
        //else
        //{
        //    Debug.Log("저장된 데이터가 없으므로 테스트 몬스터 생성");
        //    for (int i = 0; i < testMonsterList.Count; i++)
        //    {
        //        Monster m = new Monster();
        //        m.SetMonsterData(testMonsterList[i]);
        //        player.AddOwnedMonster(m);
        //        player.ToggleEntry(m);
        //        player.ToggleBattleEntry(m);
        //    }
        //}

        // 3. 엔트리 및 UI 초기화
        EntryManager.Instance.InitializeAllSlots();
        MonsterRosterManager.Instance.InitializeRoster();

        // 4. 플레이어 오브젝트 생성 및 초기화
        

      //  PlayerSaveManager.Instance.SavePlayerData(player); // 플레이어 데이터 저장
    }

    /// <summary>
    /// 플레이어 컨트롤러를 외부에서 할당합니다.
    /// </summary>
    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }

    /// <summary>
    /// 플레이어 데이터를 외부에서 할당합니다.
    /// </summary>
    public void SetPlayer(Player newPlayer)
    {
        player = newPlayer;
    }

    private void SpawnPlayerCharacter(Player loadedPlayer)
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.name = "Player";

        PlayerCharacter controller = playerInstance.GetComponent<PlayerCharacter>();

        if (controller != null)
        {
            controller.Init(loadedPlayer);

            // 위치 적용
            playerInstance.transform.position = loadedPlayer.playerLastPosition;

            // 생성한 플레이어 인스턴스의 PlayerController를 PlayerManager.playerController에 할당
            PlayerController pc = playerInstance.GetComponent<PlayerController>();
            if (pc != null)
            {
                playerController = pc;
            }
            else
            {
                Debug.LogWarning("PlayerPrefab에 PlayerController 컴포넌트가 없습니다!");
            }
        }
        else
        {
            Debug.LogError("생성된 PlayerPrefab에 PlayerCharacter 컴포넌트가 없습니다!");
        }
    }
}