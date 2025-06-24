using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    [Header("몬스터 프리팹 목록")]
    public List<GameObject> monsterPrefabs;

    [Header("Factory 부모 오브젝트")]
    public Transform factoryParent;

    [Header("BoxCollider2D로부터 계산된 스폰 영역 (읽기 전용)")]
    [SerializeField] private float width;
    [SerializeField] private float height;

    public int maxAttempts = 100;
    private List<Vector3> usedPositions = new List<Vector3>();

    private void Start()
    {
        // BoxCollider2D 기준으로 크기 자동 계산
        var collider = factoryParent.GetComponentInChildren<BoxCollider2D>();
        if (collider != null)
        {
            width = collider.size.x * factoryParent.localScale.x;
            height = collider.size.y * factoryParent.localScale.y;
            Debug.Log($"BoxCollider2D 크기 자동 설정됨: width={width}, height={height}");
        }
        else
        {
            Debug.LogWarning("BoxCollider2D를 찾지 못했습니다. Factory 오브젝트에 추가해주세요.");
        }

        // 게임 시작 시 모든 몬스터 생성
        SpawnAllMonsters();
    }

    public void SpawnAllMonsters()
    {
        if (monsterPrefabs.Count == 0 || factoryParent == null)
        {
            Debug.LogWarning("프리팹 목록 또는 부모가 설정되지 않았습니다.");
            return;
        }

        foreach (var prefab in monsterPrefabs)
        {
            Vector3 spawnPos = GetRandomPositionInFactory();
            if (spawnPos != Vector3.zero)
            {
                GameObject monster = Instantiate(prefab, spawnPos, Quaternion.identity, factoryParent);
                usedPositions.Add(spawnPos);
                Debug.Log($"{monster.name} 생성 완료 @ {spawnPos}");
            }
            else
            {
                Debug.LogWarning($"{prefab.name}을(를) 생성할 위치를 찾지 못했습니다.");
            }
        }
    }

    private Vector3 GetRandomPositionInFactory()
    {
        Vector3 center = factoryParent.position;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(-width / 2f, width / 2f);
            float y = Random.Range(-height / 2f, height / 2f);

            Vector3 candidate = center + new Vector3(x, y, 0f);

            bool isTooClose = usedPositions.Exists(pos => Vector3.Distance(pos, candidate) < 1.5f);
            if (!isTooClose)
                return candidate;
        }

        return Vector3.zero;
    }
}
