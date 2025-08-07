using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("플레이어 데이터 및 컨트롤")]
    public Player player;                       //플레이어 데이터
    public PlayerController playerController;   // 플레이어 조작 클래스
    public List<GameObject> playerPrefabs; // 0: 남캐, 1: 여캐 등
    private GameObject selectedPrefab;
    public List<Sprite> playerImage; // 플레이어 이미지

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

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMapScene")
        {
            // 1. 저장된 데이터가 있는지 시도해서 불러옴
            SpawnPlayerCharacter(player);
            if (RuntimePlayerSaveManager.Instance.HasSavedData())
            {
                RuntimePlayerSaveManager.Instance.RestoreGameState();
            }
            else
            {
                Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();

                if (loadedPlayer == null)
                {
                    player.AddItem("소형 회복 물약", 1);
                    player.AddItem("대화하기", 1);
                    SetQuestCleared();
                    SetEliteCleared();
                    SetBossCleared();
                    KeyRebinderManager.Instance.SaveCurrentBindingsToPlayer(player);
                }

            }
        }
    }

    public void SetQuestCleared()
    {
        player.playerQuestStartCheck.Add(0, false);
        player.playerQuestStartCheck.Add(1, false);
        player.playerQuestStartCheck.Add(2, false);
        player.playerQuestStartCheck.Add(3, false);
        player.playerQuestStartCheck.Add(4, false);

        player.playerQuestClearCheck.Add(0, false);
        player.playerQuestClearCheck.Add(1, false);
        player.playerQuestClearCheck.Add(2, false);
        player.playerQuestClearCheck.Add(3, false);
        player.playerQuestClearCheck.Add(4, false);
    }

    public void SetEliteCleared()
    {
        player.playerEliteStartCheck.Add(0, false);
        player.playerEliteStartCheck.Add(1, false);
        player.playerEliteStartCheck.Add(2, false);
        player.playerEliteStartCheck.Add(3, false);
        player.playerEliteStartCheck.Add(4, false);

        player.playerEliteClearCheck.Add(0, false);
        player.playerEliteClearCheck.Add(1, false);
        player.playerEliteClearCheck.Add(2, false);
        player.playerEliteClearCheck.Add(3, false);
        player.playerEliteClearCheck.Add(4, false);
    }

    public void SetBossCleared()
    {
        player.playerBossClearCheck.Add(0, false);
        player.playerBossClearCheck.Add(1, false);
        player.playerBossClearCheck.Add(2, false);
        player.playerBossClearCheck.Add(3, false);
        player.playerBossClearCheck.Add(4, false);
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
    public void SetSelectedPrefabIndex(int index)
    {
        if (index >= 0 && index < playerPrefabs.Count)
        {
            selectedPrefab = playerPrefabs[index];
        }
        else
        {
            Debug.LogWarning("잘못된 프리팹 인덱스: " + index);
        }
    }

    private void SpawnPlayerCharacter(Player loadedPlayer)
    {
        SetSelectedPrefabIndex(player.playerGender);
        GameObject playerInstance = Instantiate(selectedPrefab);

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