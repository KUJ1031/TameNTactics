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

        PlayerSaveManager.Instance.LoadPlayerData(); // 플레이어 데이터 불러오기
    }

    private void Start()
    {
        Debug.Log("PlayerManager : testMonsterList.Count = " + testMonsterList.Count);
        //test Monster add
        for(int i = 0; i < testMonsterList.Count; i++)
        {
            Monster m = new Monster();
            m.SetMonsterData(testMonsterList[i]);
            player.AddOwnedMonster(m);
            player.ToggleEntry(m);
            player.ToggleBattleEntry(m);
        }
        Debug.Log("PlayerManager : ownedMonsters.Count = " + player.ownedMonsters.Count);
        Debug.Log("PlayerManager : entryMonsters.Count = " + player.entryMonsters.Count);
        Debug.Log("PlayerManager : battleEntry.Count = " + player.battleEntry.Count);
        Debug.Log("PlayerManager : benchEntry.Count = " + player.benchEntry.Count);

        SpawnPlayerCharacter();
        // 플레이어 로스터 초기화
        MonsterRosterManager.Instance.InitializeRoster();
        // 플레이어 엔트리 초기화
        EntryManager.Instance.InitializeAllSlots();
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

    private void SpawnPlayerCharacter()
    {
        //프리팹으로 플레이어 생성
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.name = "Player";

        PlayerCharacter controller = playerInstance.GetComponent<PlayerCharacter>();

        if (controller != null)
        {
            controller.Init(this.player);
        }
        else
        {
            Debug.LogError("생성된 PlayerPrefab에 PlayerCharacter 컴포넌트가 없습니다!");
        }
    }
}