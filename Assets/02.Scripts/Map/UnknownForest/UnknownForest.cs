using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownForest : MonoBehaviour
{
    public GameObject strangeBushesPrefab;
    public int maxBushCount = 15;
    public float minBushDistance = 1f;

    public bool isQuest_FindRegueStarted = false; // 퀘스트 시작 여부
    public bool isQuest_FindRegueCleared = false; // 퀘스트 완료 여부

    private List<Vector2> spawnedPositions = new();

    private void Start()
    {
        SpawnRandomBushes();
    }

    private void SpawnRandomBushes()
    {
        Collider2D forestCollider = GetComponent<Collider2D>();
        if (forestCollider == null)
        {
            Debug.LogError("[UnknownForest] 콜라이더가 없습니다!");
            return;
        }

        Bounds bounds = forestCollider.bounds;
        int spawnAttempts = 0;

        while (spawnedPositions.Count < maxBushCount && spawnAttempts < 100)
        {
            Vector2 randomPos = GetValidRandomPosition(bounds);
            if (randomPos != Vector2.positiveInfinity)
            {
                SpawnBushAt(randomPos);
            }

            spawnAttempts++;
        }

        if (spawnedPositions.Count < maxBushCount)
        {
            Debug.LogWarning($"[UnknownForest] 생성된 수풀 수: {spawnedPositions.Count}/{maxBushCount} (충분한 공간 부족)");
        }
    }

    private Vector2 GetValidRandomPosition(Bounds bounds)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            bool tooClose = false;
            foreach (Vector2 pos in spawnedPositions)
            {
                if (Vector2.Distance(pos, randomPos) < minBushDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return randomPos;
        }

        return Vector2.positiveInfinity;
    }

    private void SpawnBushAt(Vector2 position)
    {
        GameObject bush = Instantiate(strangeBushesPrefab, position, Quaternion.identity, this.transform);
        spawnedPositions.Add(position);
    }

    public void RequestBushRespawn(Vector2 oldPos, float delay)
    {
        // 현재 위치 제거 (없으면 무시)
        spawnedPositions.Remove(oldPos);
        StartCoroutine(RespawnBushAfterDelay(delay));
    }

    private IEnumerator RespawnBushAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D forestCollider = GetComponent<Collider2D>();
        if (forestCollider == null)
            yield break;

        Bounds bounds = forestCollider.bounds;

        Vector2 newPos = GetValidRandomPosition(bounds);
        if (newPos != Vector2.positiveInfinity)
        {
            SpawnBushAt(newPos);
        }
        else
        {
            Debug.LogWarning("[UnknownForest] 재생성할 공간이 부족합니다.");
        }
    }
}
