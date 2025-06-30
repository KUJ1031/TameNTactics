using System.Collections.Generic;
using UnityEngine;

public class AllyMonsterSpawner : MonoBehaviour
{
    [Header("플레이어 몬스터 소환 위치 (최대 3)")]
    public GameObject defaultMonsterPrefab;
    public Transform[] spawnPositions;

    public void SpawnAllies(List<Monster> allies)
    {
        for (int i = 0; i < allies.Count && i < spawnPositions.Length; i++)
        {
            var data = allies[i];
            var go = Instantiate(defaultMonsterPrefab, spawnPositions[i].position, Quaternion.identity);
            var monster = go.GetComponent<Monster>();
            monster = data;

            monster.ApplyMonsterData(); // 외형 등 갱신
           // monster.LoadMonsterBaseStatData(); // 능력치 갱신
        }
    }
}
