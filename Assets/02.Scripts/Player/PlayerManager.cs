using Cinemachine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("플레이어 데이터 및 컨트롤")]
    public Player player;                       //플레이어 데이터
    public PlayerController playerController;   // 플레이어 조작 클래스
    public GameObject playerPrefab;             //실제 플레이어

    public List<MonsterData> testMonsterList; //테스트용 플레이어 몬스터들(추후 삭제)

    public CinemachineVirtualCamera virtualCamera; // 줌 아웃 카메라


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
        if (scene.name == "MainScene")
        {
            // 1. 저장된 데이터가 있는지 시도해서 불러옴
            SpawnPlayerCharacter(player);
            if (RuntimePlayerSaveManager.Instance.playerData != null)
            {
                RuntimePlayerSaveManager.Instance.RestoreGameState();
            }
            else
            {
                Player loadedPlayer = PlayerSaveManager.Instance.LoadPlayerData();

                if (loadedPlayer == null)
                {
                    Debug.Log("저장된 데이터가 없으므로 테스트 몬스터 생성");
                    for (int i = 0; i < testMonsterList.Count; i++)
                    {
                        Monster m = new Monster();
                        m.SetMonsterData(testMonsterList[i]);
                        player.AddOwnedMonster(m);
                        //player.TryAddEntryMonster(m, (success) => 
                        //    {
                        //    if (success)
                        //    {
                        //        player.AddBattleEntry(m);
                        //    }
                        //    else { Debug.Log("엔트리에 몬스터 등록 실패"); }
                        //    });
                    }
                }
            }

        }
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

        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform; // 카메라 팔로우 설정
            virtualCamera.LookAt = playerInstance.transform; // 카메라 룩앳 설정
            Debug.Log("CinemachineVirtualCamera가 설정되었습니다.");
        }

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