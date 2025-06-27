using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("플레이어 데이터 및 컨트롤")]
    public Player player;                     // 실제 플레이어 데이터
    public PlayerController playerController; // 플레이어 조작 클래스

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

    private void Start()
    {
        // 필요 시 초기화 로직 추가
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
}