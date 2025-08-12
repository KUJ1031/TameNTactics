using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownForestManager : Singleton<UnknownForestManager>
{
    public UnknownForest unknownForest;
    public MapLogicGuideUI unknownForestUI;
    public StrangeBushes currentBush;  // 현재 플레이어가 닿은 수


    public Sprite npcSprite;
    public GameObject npc;
    public Transform playerRespawnTransporm;

    [Header("StrangeBushes 재생성 설정")]
    public GameObject bushPrefab; // 재생성할 StrangeBushes 프리팹
    public List<Transform> bushSpawnPoints = new(); // 재생성 가능한 위치 목록

    public List<Vector2> savedBushPositions = new();

    /// <summary>
    /// 일정 시간 후 새로운 수풀을 랜덤 위치에 재생성합니다.
    /// </summary>
    /// <param name="oldBush">이전에 제거된 수풀</param>
    /// <param name="delay">재생성까지 대기 시간(초)</param>
    /// 
    public void RespawnBush(StrangeBushes oldBush, float delay)
    {
        StartCoroutine(RespawnCoroutine(oldBush, delay));
    }

    private IEnumerator RespawnCoroutine(StrangeBushes oldBush, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bushPrefab == null || bushSpawnPoints.Count == 0)
        {
            Debug.LogWarning("[UnknownForestManager] bushPrefab 또는 bushSpawnPoints가 설정되지 않았습니다.");
            yield break;
        }

        // 재생성 위치 랜덤 선택
        int index = Random.Range(0, bushSpawnPoints.Count);
        Transform spawnPoint = bushSpawnPoints[index];

        // Destroy 하지 말고 비활성화만 했다가 다시 활성화 및 위치 이동
        if (oldBush != null)
        {
            oldBush.gameObject.SetActive(true);
            oldBush.transform.position = spawnPoint.position;
            Debug.Log($"[미지의 숲] 덤불이 위치 {index}에서 재활성화되었습니다.");
        }
        else
        {
            // oldBush가 없으면 신규 생성 (필요시)
            Instantiate(bushPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log($"[미지의 숲] 새로운 덤불이 위치 {index}에 생성되었습니다.");
        }
    }
}
